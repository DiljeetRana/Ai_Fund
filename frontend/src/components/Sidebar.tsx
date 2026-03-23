import React from 'react';

interface SidebarProps {
  isOpen: boolean;
  onNewChat: () => void;
}

const Sidebar: React.FC<SidebarProps> = ({ isOpen, onNewChat }) => {
  return (
    <div
      className={`fixed lg:relative w-72 h-full bg-white border-r border-slate-100 flex flex-col transition-transform duration-300 z-50 ${
        isOpen ? 'translate-x-0' : '-translate-x-full lg:translate-x-0'
      }`}
    >
      {/* Logo */}
      <div className="p-6 border-b border-slate-100">
        <div className="flex items-center gap-3">
          <div className="w-10 h-10 bg-blue-600 rounded-xl flex items-center justify-center text-white text-xl shadow-lg shadow-blue-500/10">
            F
          </div>
          <span className="text-xl font-bold text-slate-900">FundAI</span>
        </div>
      </div>

      {/* New Chat Button */}
      <div className="p-5">
        <button
          onClick={onNewChat}
          className="w-full py-3.5 px-5 bg-slate-900 hover:bg-black text-white rounded-xl font-bold flex items-center justify-center gap-2 transition-all shadow-lg shadow-slate-200 active:scale-95"
        >
          <span className="text-lg">➕</span>
          <span>New Chat</span>
        </button>
      </div>

      {/* Menu */}
      <div className="flex-1 px-3 overflow-y-auto">
        <MenuItem icon="💭" label="Current Chat" active />
        <MenuItem icon="📜" label="Chat History" />
        <MenuItem icon="⚙️" label="Settings" />
      </div>
    </div>
  );
};

interface MenuItemProps {
  icon: string;
  label: string;
  active?: boolean;
}

const MenuItem: React.FC<MenuItemProps> = ({ icon, label, active }) => {
  return (
    <div
      className={`flex items-center gap-3 px-4 py-3 rounded-xl cursor-pointer transition-all my-1 ${
        active
          ? 'bg-blue-50 text-blue-600 font-bold border border-blue-100 shadow-sm shadow-blue-500/5'
          : 'text-slate-500 hover:bg-slate-50 hover:text-slate-900'
      }`}
    >
      <span className="text-lg">{icon}</span>
      <span className="text-sm">{label}</span>
    </div>
  );
};

export default Sidebar;
