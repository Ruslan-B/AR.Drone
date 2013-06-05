using System.Runtime.InteropServices;

namespace AR.Drone.Data.Navigation.Native.Options
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public unsafe struct navdata_raw_measures_t
    {
        public ushort tag;
        public ushort size;
        public fixed ushort raw_accs [3];
        public fixed short raw_gyros [3];
        public fixed short raw_gyros_110 [2];
        public uint vbat_raw;
        public ushort us_debut_echo;
        public ushort us_fin_echo;
        public ushort us_association_echo;
        public ushort us_distance_echo;
        public ushort us_courbe_temps;
        public ushort us_courbe_valeur;
        public ushort us_courbe_ref;
        public ushort flag_echo_ini;
        public ushort nb_echo;
        public uint sum_echo;
        public int alt_temp_raw;
        public short gradient;
    }
}