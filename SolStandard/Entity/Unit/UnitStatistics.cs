﻿using System;

namespace SolStandard.Entity.Unit
{
    public enum UnitParameters
    {
        Hp,
        Atk,
        Def,
        Sp,
        Ap,
        Mv,
        Rng
    }

    public class UnitStatistics
    {
        private int hp;
        private int atk;
        private int def;
        private int sp;
        private int ap;
        private int mv;
        private int[] atkRange;
        private int itv;

        private readonly int maxHp;
        private readonly int baseAtk;
        private readonly int baseDef;
        private readonly int maxSp;
        private readonly int maxAp;
        private readonly int maxMv;
        private readonly int[] baseAtkRange;
        private readonly int baseItv;


        public UnitStatistics(int hp, int atk, int def, int sp, int ap, int mv, int[] atkRange, int itv)
        {
            Hp = hp;
            Atk = atk;
            Def = def;
            Sp = sp;
            Ap = ap;
            Mv = mv;
            AtkRange = atkRange;
            Itv = itv;

            maxHp = hp;
            baseAtk = atk;
            baseDef = def;
            maxSp = sp;
            maxAp = ap;
            maxMv = mv;
            baseAtkRange = atkRange;
            baseItv = itv;
        }

        public int MaxHp
        {
            get { return maxHp; }
        }

        public int BaseAtk
        {
            get { return baseAtk; }
        }

        public int BaseDef
        {
            get { return baseDef; }
        }

        public int MaxSp
        {
            get { return maxSp; }
        }

        public int MaxAp
        {
            get { return maxAp; }
        }

        public int MaxMv
        {
            get { return maxMv; }
        }

        public int[] BaseAtkRange
        {
            get { return baseAtkRange; }
        }

        public int Hp
        {
            get { return hp; }
            set { hp = value; }
        }

        public int Atk
        {
            get { return atk; }
            set { atk = value; }
        }

        public int Def
        {
            get { return def; }
            set { def = value; }
        }

        public int Sp
        {
            get { return sp; }
            set { sp = value; }
        }

        public int Ap
        {
            get { return ap; }
            set { ap = value; }
        }

        public int Mv
        {
            get { return mv; }
            set { mv = value; }
        }

        public int[] AtkRange
        {
            get { return atkRange; }
            set { atkRange = value; }
        }

        public int Itv
        {
            get { return itv; }
            set { itv = value; }
        }

        public int BaseItv
        {
            get { return baseItv; }
        }

        public override string ToString()
        {
            string output = "";

            output += "HP: " + Hp.ToString() + "/" + maxHp;
            output += Environment.NewLine;
            output += "ATK: " + Atk.ToString() + "/" + baseAtk;
            output += Environment.NewLine;
            output += "DEF: " + Def.ToString() + "/" + baseDef;
            output += Environment.NewLine;
            output += "SP: " + Sp.ToString() + "/" + maxSp;
            output += Environment.NewLine;
            output += "AP: " + Ap.ToString() + "/" + maxAp;
            output += Environment.NewLine;
            output += "MV: " + Mv.ToString() + "/" + maxMv;
            output += Environment.NewLine;
            output += string.Format("RNG: [{0}]/[{1}]", string.Join(",", AtkRange), string.Join(",", baseAtkRange));
            output += Environment.NewLine;
            output += "ITV: " + Itv.ToString() + "/" + baseItv;

            return output;
        }
    }
}