import React, { useState, useEffect, useCallback } from 'react';
import api from '../services/api';
import { motion } from 'framer-motion';


interface MarketItem {
  value: string;
  trend: string;
  color: string;
}

interface MarketData {
  nifty: MarketItem;
  sensex: MarketItem;
  usdInr: MarketItem;
}

const InsightCard: React.FC<{ title: string; value: string; trend?: string; color: string; loading?: boolean }> = ({ title, value, trend, color, loading }) => (
  <motion.div 
    initial={{ opacity: 0, y: 10 }}
    animate={{ opacity: 1, y: 0 }}
    whileHover={{ y: -2, scale: 1.01 }}
    className="p-4 rounded-2xl border border-border-primary bg-bg-primary/40 backdrop-blur-md mb-4 hover:border-indigo-500/30 hover:shadow-xl hover:shadow-indigo-500/5 transition-all duration-300 group"
  >
    <div className="text-[10px] font-bold text-text-muted uppercase tracking-widest mb-1 group-hover:text-indigo-400 transition-colors">{title}</div>
    <div className="flex items-end justify-between">
      {loading ? (
        <div className="h-7 w-24 bg-bg-secondary/50 animate-pulse rounded-lg mt-1" />
      ) : (
        <>
          <motion.div 
            key={value}
            initial={{ opacity: 0.5 }}
            animate={{ opacity: 1 }}
            className="text-2xl font-black text-text-primary tracking-tight"
          >
            {value}
          </motion.div>
          {trend && (
            <div className={`text-[10px] font-black px-2 py-0.5 rounded-full ${color === 'green' ? 'bg-emerald-500/10 text-emerald-500 border border-emerald-500/20' : 'bg-rose-500/10 text-rose-500 border border-rose-500/20'}`}>
              {trend}
            </div>
          )}
        </>
      )}
    </div>
  </motion.div>
);

const InsightPanel: React.FC = () => {
  const [data, setData] = useState<MarketData | null>(null);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [lastUpdated, setLastUpdated] = useState<string | null>(null);

  const fetchMarketData = useCallback(async (isManual = false) => {
    if (isManual) setRefreshing(true);
    else setLoading(true);

    try {
      const response = await api.get('/Market/overview');
      setData(response.data);
      setLastUpdated(new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }));
    } catch (error) {
      console.error('Error fetching market data:', error);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  }, []);

  useEffect(() => {
    fetchMarketData();
    const interval = setInterval(() => fetchMarketData(), 60000); // Auto-refresh every minute
    return () => clearInterval(interval);
  }, [fetchMarketData]);

  return (
    <aside className="hidden lg:flex flex-col w-84 h-full border-l border-border-primary bg-bg-secondary/20 backdrop-blur-2xl p-6 overflow-y-auto scrollbar-hide">
      <div className="mb-8">
        <div className="flex items-center justify-between mb-8">
          <div className="flex flex-col">
            <h3 className="text-sm font-black text-text-primary tracking-tight flex items-center gap-2">
              Market Intelligence
            </h3>
            <div className="flex items-center gap-1.5 mt-1">
              <span className="w-1.5 h-1.5 rounded-full bg-emerald-500 animate-pulse" />
              <span className="text-[10px] font-bold text-emerald-500 uppercase tracking-tighter">Live System</span>
            </div>
          </div>
          <motion.button 
            whileHover={{ scale: 1.1, rotate: 180 }}
            whileTap={{ scale: 0.9 }}
            onClick={() => fetchMarketData(true)}
            disabled={refreshing || loading}
            className={`p-2 rounded-xl bg-bg-primary/50 border border-border-primary text-text-muted hover:text-indigo-500 hover:border-indigo-500/30 transition-all ${refreshing ? 'animate-spin' : ''}`}
            title="Refresh Market Data"
          >
            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
              <path d="M21 12a9 9 0 1 1-9-9c2.52 0 4.93 1 6.74 2.74L21 8"/>
              <path d="M21 3v5h-5"/>
            </svg>
          </motion.button>
        </div>
        
        <div className="space-y-1">
          <InsightCard 
            title="NIFTY 50" 
            value={data?.nifty.value || "24,250.35"} 
            trend={data?.nifty.trend || "+0.52%"} 
            color={data?.nifty.color || "green"} 
            loading={loading && !data}
          />
          <InsightCard 
            title="SENSEX" 
            value={data?.sensex.value || "79,486.20"} 
            trend={data?.sensex.trend || "+0.41%"} 
            color={data?.sensex.color || "green"} 
            loading={loading && !data}
          />
          <InsightCard 
            title="USD/INR" 
            value={data?.usdInr.value || "₹83.42"} 
            trend={data?.usdInr.trend || "-0.02%"} 
            color={data?.usdInr.color || "rose"} 
            loading={loading && !data}
          />
        </div>

        {lastUpdated && (
          <div className="text-[10px] text-text-muted mt-4 px-2 font-medium opacity-60">
            Updated at {lastUpdated} • Real-time global feed
          </div>
        )}
      </div>

      <div className="mt-6">
        <h3 className="text-[11px] font-black text-text-muted uppercase tracking-[0.2em] mb-4 flex items-center gap-2">
          AI INSIGHTS ⚡
        </h3>
        <div className="space-y-4">
          <motion.div 
            whileHover={{ x: 4 }}
            className="p-4 rounded-2xl bg-indigo-500/[0.03] border border-indigo-500/10 hover:border-indigo-500/30 transition-all cursor-default group"
          >
            <p className="text-xs text-text-secondary leading-relaxed group-hover:text-text-primary transition-colors">
              Your interest in <span className="text-indigo-500 font-bold">Aggressive Hybrid Funds</span> suggests a high risk appetite. 
              <br/><br/>
              <span className="text-indigo-400 font-black text-[10px] uppercase">Smart Recommendation:</span> Consider balancing with Sovereign Gold Bonds for diversity.
            </p>
          </motion.div>
          
          <div className="p-4 rounded-2xl bg-bg-primary/30 border border-dashed border-border-primary italic opacity-80 hover:opacity-100 transition-opacity">
            <p className="text-[11px] text-text-muted leading-relaxed">
              "System stability is optimal. Market volatility is currently within expected AI-monitored parameters."
            </p>
          </div>
        </div>
      </div>

      <div className="mt-auto pt-8">
        <motion.div 
          whileHover={{ y: -4 }}
          className="p-6 rounded-[2.5rem] bg-gradient-to-br from-indigo-600 via-indigo-600 to-blue-700 text-white shadow-2xl shadow-indigo-500/30 relative overflow-hidden group border border-white/10"
        >
          <div className="absolute top-0 right-0 w-32 h-32 bg-white/20 rounded-full -mr-12 -mt-12 blur-3xl group-hover:bg-white/30 transition-all duration-500" />
          <div className="absolute bottom-0 left-0 w-24 h-24 bg-blue-400/20 rounded-full -ml-8 -mb-8 blur-2xl" />
          
          <div className="relative z-10">
            <div className="flex items-center gap-2 mb-3">
              <div className="p-1.5 bg-white/20 rounded-lg backdrop-blur-md">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5">
                  <path d="m12 3-1.912 5.813a2 2 0 0 1-1.275 1.275L3 12l5.813 1.912a2 2 0 0 1 1.275 1.275L12 21l1.912-5.813a2 2 0 0 1 1.275-1.275L21 12l-5.813-1.912a2 2 0 0 1-1.275-1.275L12 3Z"/>
                </svg>
              </div>
              <h4 className="text-base font-black tracking-tight">Portfolio Pro</h4>
            </div>
            <p className="text-xs opacity-90 leading-relaxed mb-5 font-medium">
              Unlock the full potential of AI-driven risk management.
            </p>
            <button className="w-full py-3 bg-white text-indigo-700 rounded-2xl font-black text-xs hover:shadow-[0_8px_30px_rgb(0,0,0,0.12)] active:scale-95 transition-all">
              Upgrade Now
            </button>
          </div>
        </motion.div>
      </div>
    </aside>
  );
};


export default InsightPanel;

