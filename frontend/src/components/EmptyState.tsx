import React from 'react';

interface EmptyStateProps {
  onSuggestionClick: (question: string) => void;
}

const EmptyState: React.FC<EmptyStateProps> = ({ onSuggestionClick }) => {
  const suggestions = [
    { icon: '💰', text: 'Best SIP under ₹1000?' },
    { icon: '📊', text: 'FD vs SIP comparison' },
    { icon: '💼', text: 'Tax saving mutual funds' },
    { icon: '🎯', text: 'How does SIP work?' },
  ];

  return (
    <div className="flex flex-col items-center justify-center min-h-[60vh] text-center px-4">
      <div className="text-6xl mb-6">👋</div>
      <h1 className="text-3xl font-bold text-slate-900 mb-3">
        Hello! I'm FundAI
      </h1>
      <p className="text-slate-600 text-base mb-8 max-w-md">
        Your intelligent financial assistant. Ask me anything about mutual funds, SIP, investments, and returns.
      </p>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-3 max-w-2xl mt-6">
        {suggestions.map((suggestion, index) => (
          <button
            key={index}
            onClick={() => onSuggestionClick(suggestion.text)}
            className="p-4 bg-white border border-slate-200 rounded-xl text-left text-sm text-slate-600 hover:border-primary hover:shadow-md hover:-translate-y-0.5 transition-all"
          >
            <span className="mr-2">{suggestion.icon}</span>
            {suggestion.text}
          </button>
        ))}
      </div>
    </div>
  );
};

export default EmptyState;
