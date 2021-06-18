namespace Shining.VibrationSystem {

    public interface IVibrationSys {

		bool HasVibrator();
		bool HasAmplituideControl();
        void Play(Pattern p);
        void Play(Vibe p);
        void Cancel();

    }

}
