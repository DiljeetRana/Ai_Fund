import React from 'react';

interface HeaderProps {
  currentView: 'chat' | 'dashboard';
  onViewChange: (view: 'chat' | 'dashboard') => void;
}

const Header: React.FC<HeaderProps> = ({ currentView, onViewChange }) => {
  return (
    <div className="bg-white border-b border-slate-200 px-6 py-4">
      <div className="flex items-center justify-between max-w-7xl mx-auto">
        {/* Logo */}
        <div className="flex items-center gap-3">
          <div className="w-10 h-10 bg-gradient-to-br from-primary to-primary-light rounded-xl flex items-center justify-center text-white text-xl">
            💰
          </div>
          <span className="text-xl font-bold text-slate-900">FundAI</span>
        </div>

        {/* Navigation */}
        <div className="flex items-center gap-2 bg-slate-100 p-1 rounded-xl">
          <button
            onClick={() => onViewChange('chat')}
            className={`px-4 py-2 rounded-lg text-sm font-medium transition-all ${
              currentView === 'chat'
                ? 'bg-white text-slate-900 shadow-sm'
                : 'text-slate-600 hover:text-slate-900'
            }`}
          >
            💬 Chat
          </button>
          <button
            onClick={() => onViewChange('dashboard')}
            className={`px-4 py-2 rounded-lg text-sm font-medium transition-all ${
              currentView === 'dashboard'
                ? 'bg-white text-slate-900 shadow-sm'
                : 'text-slate-600 hover:text-slate-900'
            }`}
          >
            📊 Dashboard
          </button>
        </div>

        {/* User Menu */}
        <div className="flex items-center gap-3">
          <button className="w-9 h-9 flex items-center justify-center text-slate-600 hover:bg-slate-100 rounded-lg transition-all">
            🔔
          </button>
          <button className="w-9 h-9 flex items-center justify-center text-slate-600 hover:bg-slate-100 rounded-lg transition-all">
            ⚙️
          </button>
          <div className="w-9 h-9 bg-primary rounded-lg flex items-center justify-center text-white font-semibold">
            M
          </div>
        </div>
      </div>
    </div>
  );
};

export default Header;
