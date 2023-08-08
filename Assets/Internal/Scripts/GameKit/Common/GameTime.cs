using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using JetBrains.Annotations;
using UnityEngine;

namespace Karabaev.GameKit.Common
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    [PublicAPI]
    public readonly struct GameTime
    {
        public float Seconds { get; }

        public static GameTime Now => new(Time.time);

        public static GameTime Min { get; } = new(float.MinValue);

        public static GameTime Max { get; } = new(float.MaxValue);

        public GameTime AddSeconds(float seconds) => new(Seconds + seconds);

        public GameTime AddSeconds(double seconds) => new(Seconds + (float) seconds);

        public GameTime Add(TimeSpan timeSpan) => new((float) (Seconds + timeSpan.TotalSeconds));

        public static GameTime FromSeconds(float seconds) => new(seconds);

        public override string ToString() => Seconds.ToString(CultureInfo.InvariantCulture);

        public bool Equals(GameTime other) => this == other;

        public override bool Equals(object? obj) => obj is GameTime other && this == other;

        public override int GetHashCode() => Seconds.GetHashCode();

        public static bool operator ==(GameTime a, GameTime b) => a.Seconds == b.Seconds;

        public static bool operator !=(GameTime a, GameTime b) => !(a == b);

        public static bool operator >(GameTime a, GameTime b) => a.Seconds > b.Seconds;

        public static bool operator <(GameTime a, GameTime b) => a.Seconds < b.Seconds;

        public static bool operator >=(GameTime a, GameTime b) => a.Seconds >= b.Seconds;

        public static bool operator <=(GameTime a, GameTime b) => a.Seconds <= b.Seconds;

        public static GameTime operator -(GameTime a, GameTime b) => new(a.Seconds - b.Seconds);

        public static GameTime operator +(GameTime a, GameTime b) => new(a.Seconds + b.Seconds);

        private GameTime(float seconds) => Seconds = seconds;
    }
}