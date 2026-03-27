import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import api from '../../services/api';
import { motion, AnimatePresence } from 'framer-motion';
import { User, Mail, Lock, AlertCircle, ArrowRight, Fingerprint } from 'lucide-react';

const Register: React.FC = () => {
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);
  const { login } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setIsSubmitting(true);

    try {
      const response = await api.post('/auth/register', { username, email, password });
      login(response.data.token, { 
        username: response.data.username, 
        email: response.data.email,
        role: response.data.role || 'User' 
      });

      navigate('/');
    } catch (err: any) {
      setError(err.response?.data?.message || 'Registration failed. Please try again.');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-bg-primary p-6 relative overflow-hidden font-['Inter']">
      {/* Background Ambience */}
      <div className="absolute top-[-10%] right-[-10%] w-[40%] h-[40%] bg-indigo-500/10 blur-[120px] rounded-full animate-pulse" />
      <div className="absolute bottom-[-10%] left-[-10%] w-[40%] h-[40%] bg-emerald-500/10 blur-[120px] rounded-full animate-pulse decoration-3000" />

      <motion.div 
        initial={{ opacity: 0, scale: 0.95 }}
        animate={{ opacity: 1, scale: 1 }}
        transition={{ duration: 0.6, ease: [0.23, 1, 0.32, 1] }}
        className="max-w-md w-full glass-panel p-10 md:p-12 rounded-[2.5rem] shadow-[0_32px_64px_rgba(0,0,0,0.2)] border border-white/10 relative z-10 backdrop-blur-3xl"
      >
        <div className="text-center mb-10">
          <motion.div 
            whileHover={{ scale: 1.1, rotate: 5 }}
            className="inline-flex items-center justify-center w-20 h-20 bg-gradient-to-br from-emerald-500 to-indigo-600 text-white rounded-[2rem] mb-6 shadow-xl shadow-emerald-500/20"
          >
            <Fingerprint size={40} strokeWidth={2.5} />
          </motion.div>
          <h1 className="text-4xl font-black text-text-primary mb-3 tracking-tighter">Join FundAI</h1>
          <p className="text-text-muted font-black uppercase tracking-[0.2em] text-[10px] opacity-60">Initialize Your Expert Profile</p>
        </div>

        <AnimatePresence>
          {error && (
            <motion.div 
              initial={{ opacity: 0, y: -10 }}
              animate={{ opacity: 1, y: 0 }}
              className="mb-8 p-4 bg-rose-500/10 border border-rose-500/20 text-rose-500 rounded-2xl flex items-center gap-3 text-[13px] font-bold"
            >
              <AlertCircle size={18} />
              {error}
            </motion.div>
          )}
        </AnimatePresence>

        <form onSubmit={handleSubmit} className="space-y-6">
          <div className="space-y-2">
            <label className={`block text-[11px] font-black uppercase tracking-[0.15em] ml-1 transition-colors duration-300 ${error ? 'text-rose-500' : 'text-text-muted'}`}>
              Username
            </label>
            <div className="relative group">
              <User className={`absolute left-4 top-1/2 -translate-y-1/2 transition-colors ${error ? 'text-rose-500' : 'text-text-muted group-focus-within:text-emerald-500'}`} size={18} />
              <input
                type="text"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                className={`w-full bg-bg-secondary/30 border rounded-2xl py-4 pl-12 pr-4 focus:outline-none focus:ring-4 transition-all font-bold text-text-primary placeholder:text-text-muted/30 ${
                  error 
                    ? 'border-rose-500/50 focus:ring-rose-500/10 bg-rose-500/5' 
                    : 'border-border-primary focus:ring-emerald-500/10 focus:border-emerald-500'
                }`}
                placeholder="Choose a professional handle"
                required
              />
            </div>
          </div>

          <div className="space-y-2">
            <label className={`block text-[11px] font-black uppercase tracking-[0.15em] ml-1 transition-colors duration-300 ${error ? 'text-rose-500' : 'text-text-muted'}`}>
              Work Email
            </label>
            <div className="relative group">
              <Mail className={`absolute left-4 top-1/2 -translate-y-1/2 transition-colors ${error ? 'text-rose-500' : 'text-text-muted group-focus-within:text-emerald-500'}`} size={18} />
              <input
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                className={`w-full bg-bg-secondary/30 border rounded-2xl py-4 pl-12 pr-4 focus:outline-none focus:ring-4 transition-all font-bold text-text-primary placeholder:text-text-muted/30 ${
                  error 
                    ? 'border-rose-500/50 focus:ring-rose-500/10 bg-rose-500/5' 
                    : 'border-border-primary focus:ring-emerald-500/10 focus:border-emerald-500'
                }`}
                placeholder="name@company.com"
                required
              />
            </div>
          </div>

          <div className="space-y-2">
            <label className={`block text-[11px] font-black uppercase tracking-[0.15em] ml-1 transition-colors duration-300 ${error ? 'text-rose-500' : 'text-text-muted'}`}>
              Security Key
            </label>
            <div className="relative group">
              <Lock className={`absolute left-4 top-1/2 -translate-y-1/2 transition-colors ${error ? 'text-rose-500' : 'text-text-muted group-focus-within:text-emerald-500'}`} size={18} />
              <input
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className={`w-full bg-bg-secondary/30 border rounded-2xl py-4 pl-12 pr-4 focus:outline-none focus:ring-4 transition-all font-bold text-text-primary placeholder:text-text-muted/30 ${
                  error 
                    ? 'border-rose-500/50 focus:ring-rose-500/10 bg-rose-500/5' 
                    : 'border-border-primary focus:ring-emerald-500/10 focus:border-emerald-500'
                }`}
                placeholder="••••••••"
                required
              />
              <div className="absolute right-4 top-1/2 -translate-y-1/2 text-[10px] font-black text-text-muted/50 uppercase tracking-widest pointer-events-none">
                Min 8 Chars
              </div>
            </div>
          </div>

          <motion.button
            whileHover={{ y: -2 }}
            whileTap={{ scale: 0.98 }}
            type="submit"
            disabled={isSubmitting}
            className="w-full py-4.5 bg-gradient-to-br from-emerald-600 to-indigo-700 text-white rounded-2xl font-black text-sm shadow-xl shadow-emerald-500/20 hover:shadow-emerald-500/40 transition-all flex items-center justify-center gap-2 group mt-8 relative overflow-hidden"
          >
            <div className="absolute inset-0 bg-white/10 translate-y-full group-hover:translate-y-0 transition-transform duration-300 pointer-events-none" />
            <span className="relative z-10">{isSubmitting ? 'Securing Profile...' : 'Create Master Account'}</span>
            {!isSubmitting && <ArrowRight size={18} className="relative z-10 group-hover:translate-x-1 transition-transform" />}
          </motion.button>
        </form>

        <div className="mt-10 pt-8 border-t border-white/5 flex flex-col items-center gap-4">
          <p className="text-text-muted text-[10px] font-black uppercase tracking-[0.2em] opacity-40">
            Already registered?
          </p>
          <Link to="/login" className="px-6 py-2.5 rounded-xl border border-border-primary text-xs font-black text-text-primary hover:bg-bg-secondary hover:border-emerald-500/30 transition-all">
            Access Vault
          </Link>
        </div>
      </motion.div>
    </div>
  );
};

export default Register;
