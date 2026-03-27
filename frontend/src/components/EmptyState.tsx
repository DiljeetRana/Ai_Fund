import React from 'react';

const EmptyState: React.FC = () => {
  return (
    <div className="flex flex-col items-center justify-center min-h-[50vh] text-center px-4 animate-fadeIn">
      <div className="w-24 h-24 rounded-[32px] bg-indigo-500/10 flex items-center justify-center text-5xl mb-10 shadow-indigo-500/5 ring-1 ring-indigo-500/20">
        ✨
      </div>
      <h1 className="text-4xl font-extrabold text-text-primary mb-6 tracking-tight">
        How can I help you today?
      </h1>
      <p className="text-text-secondary text-lg mb-10 max-w-lg leading-relaxed font-medium">
        I'm your intelligent investment companion. Ask me anything about mutual funds, SIP strategies, or market trends.
      </p>
    </div>
  );
};


export default EmptyState;
