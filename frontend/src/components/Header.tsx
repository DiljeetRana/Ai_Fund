import React from 'react';

interface HeaderProps {
  currentView: 'chat' | 'dashboard';
  onViewChange: (view: 'chat' | 'dashboard') => void;
}

const Header: React.FC<HeaderProps> = ({ currentView, onViewChange }) => {
  return (
    <div className="bg-white/80 backdrop-blur-md border-b border-slate-100 z-50 sticky top-0">
      <div className="flex items-center justify-between max-w-7xl mx-auto px-6 py-3">
        {/* Logo */}
        <div className="flex items-center gap-2.5">
          <div className="w-9 h-9 rounded-xl flex items-center justify-center text-lg bg-blue-600 shadow-lg shadow-blue-500/20 text-white font-bold">
            F
          </div>
          <span className="text-xl font-bold tracking-tight text-slate-900">
            FundAI
          </span>
        </div>

        {/* Navigation */}
        <div className="flex items-center gap-1 bg-slate-50 p-1 rounded-xl border border-slate-100">
          <button
            onClick={() => onViewChange('chat')}
            className={`px-4 py-1.5 rounded-lg text-sm font-semibold transition-all duration-200 ${
              currentView === 'chat'
                ? 'bg-white text-blue-600 shadow-sm border border-slate-200'
                : 'text-slate-500 hover:text-slate-700 hover:bg-slate-100'
            }`}
          >
            Chat
          </button>
          <button
            onClick={() => onViewChange('dashboard')}
            className={`px-4 py-1.5 rounded-lg text-sm font-semibold transition-all duration-200 ${
              currentView === 'dashboard'
                ? 'bg-white text-blue-600 shadow-sm border border-slate-200'
                : 'text-slate-500 hover:text-slate-700 hover:bg-slate-100'
            }`}
          >
            Dashboard
          </button>
        </div>
      </div>
    </div>
  );
};

export default Header;
