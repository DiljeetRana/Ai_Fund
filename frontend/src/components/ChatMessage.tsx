import React from 'react';
import { Message } from '../types';

interface ChatMessageProps {
  message: Message;
  onCopy: (content: string) => void;
  onRegenerate: () => void;
}

const ChatMessage: React.FC<ChatMessageProps> = ({ message, onCopy, onRegenerate }) => {
  const formatTime = (date: Date) => {
    return new Date(date).toLocaleTimeString('en-US', {
      hour: '2-digit',
      minute: '2-digit',
    });
  };

  return (
    <div
      className={`flex mb-8 animate-fadeIn ${
        message.isUser ? 'justify-end' : 'justify-start'
      }`}
    >
      <div className={`max-w-[85%] md:max-w-[75%] ${message.isUser ? 'items-end' : 'items-start'} flex flex-col`}>
        {/* Message Bubble */}
        <div
          className={`px-5 py-4 rounded-2xl text-[15px] leading-relaxed ${
            message.isUser
              ? 'bg-slate-900 text-white rounded-tr-sm shadow-md shadow-slate-200'
              : 'bg-white border border-slate-200 text-slate-800 rounded-tl-sm shadow-sm'
          }`}
        >
          <div className="whitespace-pre-wrap">{message.content}</div>
        </div>

        {/* Metadata & Actions */}
        {!message.isUser && (
          <div className="flex flex-col gap-3 mt-3 ml-1">
            <div className="flex items-center gap-3 text-[11px] font-medium uppercase tracking-wider text-slate-400">
              {message.source && (
                <span className="px-2 py-0.5 bg-slate-100 rounded text-slate-500 border border-slate-200/50">
                  {message.source} {message.confidence && `• ${Math.round(message.confidence * 100)}%`}
                </span>
              )}
              <span>{formatTime(message.timestamp)}</span>
            </div>

            {/* Actions */}
            <div className="flex gap-2">
              <button
                onClick={() => onCopy(message.content)}
                className="flex items-center gap-1.5 px-2.5 py-1 text-xs font-medium bg-white border border-slate-200 rounded-lg text-slate-500 hover:text-slate-900 hover:border-slate-300 transition-all shadow-sm active:scale-95"
              >
                <span>📋</span> Copy
              </button>
              <button
                onClick={onRegenerate}
                className="flex items-center gap-1.5 px-2.5 py-1 text-xs font-medium bg-white border border-slate-200 rounded-lg text-slate-500 hover:text-slate-900 hover:border-slate-300 transition-all shadow-sm active:scale-95"
              >
                <span>🔄</span> Regenerate
              </button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default ChatMessage;
