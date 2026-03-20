import React, { useState, KeyboardEvent } from 'react';

interface ChatInputProps {
  onSend: (message: string) => void;
  disabled: boolean;
}

const ChatInput: React.FC<ChatInputProps> = ({ onSend, disabled }) => {
  const [input, setInput] = useState('');

  const suggestions = [
    'What is SIP?',
    'Best mutual funds 2024',
    'Calculate SIP returns',
  ];

  const handleSend = () => {
    if (input.trim() && !disabled) {
      onSend(input.trim());
      setInput('');
    }
  };

  const handleKeyPress = (e: KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter' && !e.shiftKey) {
      e.preventDefault();
      handleSend();
    }
  };

  const handleSuggestionClick = (suggestion: string) => {
    setInput(suggestion);
  };

  return (
    <div className="border-t border-slate-200 bg-white p-5">
      <div className="max-w-3xl mx-auto">
        {/* Suggestion Chips */}
        <div className="flex flex-wrap gap-2 mb-3">
          {suggestions.map((suggestion, index) => (
            <button
              key={index}
              onClick={() => handleSuggestionClick(suggestion)}
              className="px-4 py-2 bg-white border border-slate-200 rounded-full text-xs text-slate-600 hover:border-primary hover:text-primary hover:bg-slate-50 transition-all"
            >
              {suggestion}
            </button>
          ))}
        </div>

        {/* Input Box */}
        <div className="flex gap-3 items-end">
          <div className="flex-1 flex items-center bg-white border-2 border-slate-200 rounded-2xl px-5 py-1 focus-within:border-primary focus-within:shadow-lg focus-within:shadow-primary/10 transition-all">
            <input
              type="text"
              value={input}
              onChange={(e) => setInput(e.target.value)}
              onKeyPress={handleKeyPress}
              placeholder="Ask about mutual funds, SIP, returns..."
              disabled={disabled}
              className="flex-1 py-3 text-[15px] text-slate-900 placeholder-slate-400 outline-none bg-transparent"
            />
            <button
              onClick={handleSend}
              disabled={disabled || !input.trim()}
              className="w-11 h-11 bg-primary hover:bg-primary-dark disabled:opacity-50 disabled:cursor-not-allowed text-white rounded-xl flex items-center justify-center transition-all hover:scale-105 ml-2"
            >
              <span className="text-lg">➤</span>
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ChatInput;
