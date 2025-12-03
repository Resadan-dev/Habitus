import React, { useEffect, useState } from 'react';
import { api } from '../api/api';
import { useSelector } from 'react-redux';
import type { RootState } from '../store/store';
import { Shield, Zap, Brain, Trophy } from 'lucide-react';

interface Stats {
    strength: number;
    intellect: number;
    stamina: number;
}

interface Player {
    id: string;
    xp: number;
    level: number;
    stats: Stats;
}

interface PlayerStats {
    level: number;
    xp: number;
    strength: number;
    intellect: number;
    stamina: number;
}

export const CharacterStats: React.FC = () => {
    const [stats, setStats] = useState<PlayerStats | null>(null);
    const userId = useSelector((state: RootState) => state.user.userId);

    const fetchStats = async () => {
        try {
            const data = await api.fetch(`/api/players/${userId}`) as Player;
            if (data) {
                setStats({
                    level: data.level,
                    xp: data.xp,
                    strength: data.stats.strength,
                    intellect: data.stats.intellect,
                    stamina: data.stats.stamina
                });
            }
        } catch (error) {
            console.error('Failed to fetch player stats:', error);
        }
    };

    useEffect(() => {
        fetchStats();
        const interval = setInterval(fetchStats, 10000);
        return () => clearInterval(interval);
    }, [userId]);

    if (!stats) return null;

    const nextLevelXp = Math.floor(100 * Math.pow(stats.level, 1.5));
    const progress = Math.min(100, (stats.xp / nextLevelXp) * 100);

    return (
        <div className="relative overflow-hidden bg-card/50 backdrop-blur-md border border-white/20 rounded-2xl p-8 shadow-2xl">
            <div className="absolute top-0 left-0 w-full h-1 bg-gradient-to-r from-transparent via-primary to-transparent opacity-50" />

            <div className="flex flex-col md:flex-row items-center gap-8 md:gap-12">
                {/* Level Circle */}
                <div className="relative group">
                    <div className="absolute inset-0 bg-primary/30 rounded-full blur-xl group-hover:blur-2xl transition-all duration-500" />
                    <div className="relative w-24 h-24 rounded-full bg-background flex items-center justify-center border-4 border-primary shadow-[0_0_30px_-5px_var(--color-primary)] ring-4 ring-primary/20">
                        <span className="text-4xl font-black text-foreground">{stats.level}</span>
                    </div>
                    <div className="absolute -bottom-3 w-full text-center">
                        <span className="bg-primary text-primary-foreground text-xs font-bold px-3 py-1 rounded-full shadow-lg tracking-wider">
                            LEVEL
                        </span>
                    </div>
                </div>

                {/* XP Bar */}
                <div className="flex-1 w-full space-y-3">
                    <div className="flex justify-between items-end">
                        <div>
                            <h3 className="text-lg font-bold text-foreground">Experience Progress</h3>
                            <p className="text-sm text-muted-foreground">Keep pushing to reach the next level</p>
                        </div>
                        <div className="text-right">
                            <span className="text-2xl font-bold text-primary">{stats.xp}</span>
                            <span className="text-muted-foreground font-medium"> / {nextLevelXp} XP</span>
                        </div>
                    </div>
                    <div className="h-4 bg-secondary/50 rounded-full overflow-hidden backdrop-blur-sm border border-white/5">
                        <div
                            className="h-full bg-gradient-to-r from-primary to-purple-400 shadow-[0_0_20px_rgba(124,58,237,0.5)] transition-all duration-1000 ease-out relative"
                            style={{ width: `${progress}%` }}
                        >
                            <div className="absolute inset-0 bg-white/20 animate-[shimmer_2s_infinite]" />
                        </div>
                    </div>
                </div>

                {/* Stats Grid */}
                <div className="grid grid-cols-3 gap-4 w-full md:w-auto min-w-[300px]">
                    <div className="flex flex-col items-center p-4 bg-secondary/30 rounded-xl border border-white/10 hover:bg-secondary/50 transition-colors group">
                        <div className="p-2 bg-blue-500/10 rounded-lg mb-2 group-hover:bg-blue-500/20 transition-colors">
                            <Shield className="text-blue-500" size={24} />
                        </div>
                        <span className="text-2xl font-bold text-foreground">{stats.strength}</span>
                        <span className="text-xs font-bold text-muted-foreground uppercase tracking-wider">Strength</span>
                    </div>
                    <div className="flex flex-col items-center p-4 bg-secondary/30 rounded-xl border border-white/10 hover:bg-secondary/50 transition-colors group">
                        <div className="p-2 bg-purple-500/10 rounded-lg mb-2 group-hover:bg-purple-500/20 transition-colors">
                            <Brain className="text-purple-500" size={24} />
                        </div>
                        <span className="text-2xl font-bold text-foreground">{stats.intellect}</span>
                        <span className="text-xs font-bold text-muted-foreground uppercase tracking-wider">Intellect</span>
                    </div>
                    <div className="flex flex-col items-center p-4 bg-secondary/30 rounded-xl border border-white/10 hover:bg-secondary/50 transition-colors group">
                        <div className="p-2 bg-yellow-500/10 rounded-lg mb-2 group-hover:bg-yellow-500/20 transition-colors">
                            <Zap className="text-yellow-500" size={24} />
                        </div>
                        <span className="text-2xl font-bold text-foreground">{stats.stamina}</span>
                        <span className="text-xs font-bold text-muted-foreground uppercase tracking-wider">Stamina</span>
                    </div>
                </div>
            </div>
        </div>
    );
};
