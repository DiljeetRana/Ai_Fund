import React from 'react';

const EmptyState: React.FC = () => {
    // Shortcut buttons removed

  return (
    <div className="flex flex-col items-center justify-center min-h-[60vh] text-center px-4 animate-fadeIn">
      <div className="w-20 h-20 rounded-3xl bg-blue-50 flex items-center justify-center text-4xl mb-8 shadow-sm border border-blue-100">
        📊
      </div>
      <h1 className="text-4xl font-extrabold text-slate-900 mb-4 tracking-tight">
        Welcome to FundAI
      </h1>
      <p className="text-slate-500 text-lg mb-8 max-w-lg leading-relaxed font-medium">
        Your professional investment companion. Ask me about mutual funds, portfolio strategies, or market insights.
      </p>
    </div>
  );
};

export default EmptyState;
