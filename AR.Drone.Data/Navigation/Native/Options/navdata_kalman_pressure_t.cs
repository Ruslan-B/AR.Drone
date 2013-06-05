using System.Runtime.InteropServices;

namespace AR.Drone.Data.Navigation.Native.Options
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct navdata_kalman_pressure_t
    {
        public ushort tag;
        public ushort size;
        public float offset_pressure;
        public float est_z;
        public float est_zdot;
        public float est_bias_PWM;
        public float est_biais_pression;
        public float offset_US;
        public float prediction_US;
        public float cov_alt;
        public float cov_PWM;
        public float cov_vitesse;
        public int bool_effet_sol;
        public float somme_inno;
        public int flag_rejet_US;
        public float u_multisinus;
        public float gaz_altitude;
        public int Flag_multisinus;
        public int Flag_multisinus_debut;
    }
}