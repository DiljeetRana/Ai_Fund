import React from 'react';

const TypingIndicator: React.FC = () => {
  return (
    <div className="flex mb-6 animate-fadeIn">
      <div className="flex gap-1 px-4 py-3.5 bg-white border border-slate-200 rounded-2xl rounded-bl-sm">
        <div className="w-2 h-2 bg-slate-400 rounded-full animate-bounce" style={{ animationDelay: '0ms' }} />
        <div className="w-2 h-2 bg-slate-400 rounded-full animate-bounce" style={{ animationDelay: '150ms' }} />
        <div className="w-2 h-2 bg-slate-400 rounded-full animate-bounce" style={{ animationDelay: '300ms' }} />
      </div>
    </div>
  );
};

export default TypingIndicator;
