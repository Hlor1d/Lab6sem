using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace WpfLibrary1
{


    public struct forfunc
    {
        [DllImport("..\\..\\..\\..\\x64\\DEBUG\\Dll1.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern
        void funccpp(int n, int mytype, double[] x, double[] rez, ref double mytime, ref int ret);
    }

    public class VMGrid
    {
        public int VMf { get; set; } //1 = TAN, 3=ErfInv
        public int arglen { get; set; }
        public double vecbeg { get; set; }
        public double vecend { get; set; }
        public double gridstep { get { return (vecend - vecbeg) / (arglen - 1); } }
        public override string ToString()
        {
            return this.VMf.ToString() + '\n' + this.arglen.ToString() + '\n' + this.vecbeg.ToString() + '\n' + this.vecend.ToString() + '\n';
        }
    }

    public struct VMTime
    {
        public VMGrid grid { get; set; }
        public double timeHa { get; set; }
        public double timeEP { get; set; }
        public double timeC { get; set; }
        public double timeHAtoC { get; set; }
        public double timeEPtoC { get; set; }
        public override string ToString()
        {
            return this.grid.ToString() + this.timeEP.ToString(("0." + new string('#', 339))) +
                '\n' + this.timeHa.ToString(("0." + new string('#', 339)))
                + '\n' + this.timeC.ToString(("0." + new string('#', 339))) +
                '\n' + this.timeEPtoC.ToString(("0." + new string('#', 339))) +
            '\n' + this.timeHAtoC.ToString(("0." + new string('#', 339))) + '\n';
        }
    }

    public struct VMAccuracy
    {
        public VMGrid grid { get; set; }
        public double maxdif { get; set; }
        public double maxdifarg { get; set; }
        public double maxdifHa { get; set; }
        public double maxdifEP { get; set; }
        public override string ToString()
        {
            return this.grid.ToString() +
                this.maxdif.ToString(("0." + new string('#', 339))) +
                '\n' + this.maxdifarg.ToString(("0." + new string('#', 339))) +
                '\n' + this.maxdifHa.ToString(("0." + new string('#', 339))) +
                '\n' + this.maxdifEP.ToString(("0." + new string('#', 339))) + '\n';
        }
    }

    public class VMBenchmark
    {
        public VMBenchmark()
        {
            timecompamm = 0;
            acccompamm = 0;
        }
        public int timecompamm;
        public int acccompamm;
        public ObservableCollection<VMTime> timecomp = new ObservableCollection<VMTime>();
        public ObservableCollection<VMAccuracy> acccomp = new ObservableCollection<VMAccuracy>();
        public void AddVMTime(VMGrid mygrid)
        {
            if (timecompamm == 0)
            {
                minHAtoC = Double.MaxValue;
                minEPtoC = Double.MaxValue;
            }
            timecompamm++;
            double[] x = new double[mygrid.arglen];
            for (int i = 0; i < mygrid.arglen; i++)
            {
                x[i] = mygrid.vecbeg + i * (mygrid.vecend - mygrid.vecbeg) / (mygrid.arglen - 1);
            }

            double[] rezEP = new double[mygrid.arglen];
            double mytime = 0;
            int mytype = mygrid.VMf;
            int ret = 0;
            forfunc.funccpp(mygrid.arglen, mytype, x, rezEP, ref mytime, ref ret);
            for (int i = 0; i < mygrid.arglen; i++)
            {
                rezEP[i]++;
            }
            VMTime vmtimenew = new VMTime();

            vmtimenew.grid = mygrid;
            vmtimenew.timeEP = mytime;

            mytime = 0;
            ret = 0;
            double[] rezHA = new double[mygrid.arglen];
            forfunc.funccpp(mygrid.arglen, mytype + 1, x, rezHA, ref mytime, ref ret);
            vmtimenew.timeHa = mytime;

            mytime = 0;
            ret = 0;
            double[] rezC = new double[mygrid.arglen];
            forfunc.funccpp(mygrid.arglen, 5, x, rezC, ref mytime, ref ret);
            vmtimenew.timeC = mytime;

            vmtimenew.timeEPtoC = vmtimenew.timeEP / vmtimenew.timeC;
            vmtimenew.timeHAtoC = vmtimenew.timeHa / vmtimenew.timeC;
            if (vmtimenew.timeEPtoC < this.minEPtoC)
            {
                this.minEPtoC = vmtimenew.timeEPtoC;
            }
            if (vmtimenew.timeHAtoC < this.minHAtoC)
            {
                this.minHAtoC = vmtimenew.timeHAtoC;
            }

            timecomp.Add(vmtimenew);
        }
        public void AddVMAccuracy(VMGrid mygrid)
        {
            acccompamm++;
            double[] x = new double[mygrid.arglen];
            for (int i = 0; i < mygrid.arglen; i++)
            {
                x[i] = mygrid.vecbeg + i * (mygrid.vecend - mygrid.vecbeg) / (mygrid.arglen - 1);
            }

            double[] rez = new double[mygrid.arglen];
            double mytime = 0;
            int mytype = mygrid.VMf;
            int ret = 0;
            forfunc.funccpp(mygrid.arglen, mytype, x, rez, ref mytime, ref ret);

            VMAccuracy vmaccuracynew = new VMAccuracy();

            vmaccuracynew.grid = mygrid;

            double[] rez2 = new double[mygrid.arglen];
            ret = 0;
            forfunc.funccpp(mygrid.arglen, mytype + 1, x, rez2, ref mytime, ref ret);

            int maxid = 0;
            double maxval = 0, maxha = 0, maxep = 0;
            for (int i = 0; i < mygrid.arglen; i++)
            {
                if (Math.Abs(rez[i] - rez2[i]) > maxval)
                {
                    maxval = Math.Abs(rez[i] - rez2[i]);
                    maxid = i;
                    maxep = rez[i];
                    maxha = rez2[i];
                }
            }
            vmaccuracynew.maxdif = maxval;
            vmaccuracynew.maxdifarg = maxid;
            vmaccuracynew.maxdifHa = maxha;
            vmaccuracynew.maxdifEP = maxep;

            acccomp.Add(vmaccuracynew);
        }

        public void AddVMAccuracy(VMAccuracy a)
        {
            acccomp.Add(a);
            acccompamm++;
        }

        public void AddVMTime(VMTime a)
        {
            timecomp.Add(a);
            timecompamm++;
        }
        public double minHAtoC { get; set; } = 0;
        public double minEPtoC { get; set; } = 0;
        public override string ToString()
        {
            string mystr = "";
            mystr += this.timecompamm.ToString() + '\n';
            for (int i = 0; i < timecompamm; i++)
            {
                mystr += this.timecomp[i].ToString();
            }
            mystr += this.acccompamm.ToString() + '\n';
            for (int i = 0; i < acccompamm; i++)
            {
                mystr += this.acccomp[i].ToString();
            }
            mystr += minHAtoC.ToString(("0." + new string('#', 339))) + '\n' + minEPtoC.ToString(("0." + new string('#', 339)));

            return mystr;
        }
    }
}