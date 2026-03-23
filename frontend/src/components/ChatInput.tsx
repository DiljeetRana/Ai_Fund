import React, { useState, KeyboardEvent } from 'react';

interface ChatInputProps {
  onSend: (message: string) => void;
  disabled: boolean;
}

const ChatInput: React.FC<ChatInputProps> = ({ onSend, disabled }) => {
  const [input, setInput] = useState('');

  // Suggestion chips removed

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



  return (
    <div className="w-full flex justify-center pb-6">
      <div className="w-full max-w-3xl px-4">
        {/* Input Box */}
        <div className="relative flex items-center bg-white border border-slate-200 rounded-2xl p-1.5 shadow-sm hover:border-slate-300 transition-all focus-within:border-blue-500 focus-within:shadow-md focus-within:shadow-blue-500/5">
          <input
            type="text"
            value={input}
            onChange={(e) => setInput(e.target.value)}
            onKeyPress={handleKeyPress}
            placeholder="Ask anything about mutual funds..."
            disabled={disabled}
            className="flex-1 py-3 px-5 text-[15px] text-slate-900 placeholder-slate-400 outline-none bg-transparent"
          />
          <button
            onClick={handleSend}
            disabled={disabled || !input.trim()}
            className="w-10 h-10 md:w-11 md:h-11 bg-slate-900 hover:bg-black disabled:opacity-20 disabled:grayscale text-white rounded-xl flex items-center justify-center transition-all active:scale-95 shadow-lg shadow-slate-200"
          >
            <span className="text-lg">➤</span>
          </button>
        </div>
        <p className="text-[10px] text-center text-slate-400 mt-3 font-medium uppercase tracking-wider">
          FundAI can make mistakes. Check important info.
        </p>
      </div>
    </div>
  );
};

export default ChatInput;
