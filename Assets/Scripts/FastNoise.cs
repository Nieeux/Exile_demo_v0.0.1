using System;
using System.Runtime.CompilerServices;

public class FastNoise
{
	public FastNoise(int seed = 1337)
	{
		this.m_seed = seed;
		this.CalculateFractalBounding();
	}

	public static float GetDecimalType()
	{
		return 0f;
	}

	public int GetSeed()
	{
		return this.m_seed;
	}

	public void SetSeed(int seed)
	{
		this.m_seed = seed;
	}

	public void SetFrequency(float frequency)
	{
		this.m_frequency = frequency;
	}

	public void SetInterp(FastNoise.Interp interp)
	{
		this.m_interp = interp;
	}

	public void SetNoiseType(FastNoise.NoiseType noiseType)
	{
		this.m_noiseType = noiseType;
	}

	public void SetFractalOctaves(int octaves)
	{
		this.m_octaves = octaves;
		this.CalculateFractalBounding();
	}

	public void SetFractalLacunarity(float lacunarity)
	{
		this.m_lacunarity = lacunarity;
	}

	public void SetFractalGain(float gain)
	{
		this.m_gain = gain;
		this.CalculateFractalBounding();
	}

	public void SetFractalType(FastNoise.FractalType fractalType)
	{
		this.m_fractalType = fractalType;
	}

	public void SetCellularDistanceFunction(FastNoise.CellularDistanceFunction cellularDistanceFunction)
	{
		this.m_cellularDistanceFunction = cellularDistanceFunction;
	}

	public void SetCellularReturnType(FastNoise.CellularReturnType cellularReturnType)
	{
		this.m_cellularReturnType = cellularReturnType;
	}

	public void SetCellularDistance2Indicies(int cellularDistanceIndex0, int cellularDistanceIndex1)
	{
		this.m_cellularDistanceIndex0 = Math.Min(cellularDistanceIndex0, cellularDistanceIndex1);
		this.m_cellularDistanceIndex1 = Math.Max(cellularDistanceIndex0, cellularDistanceIndex1);
		this.m_cellularDistanceIndex0 = Math.Min(Math.Max(this.m_cellularDistanceIndex0, 0), 3);
		this.m_cellularDistanceIndex1 = Math.Min(Math.Max(this.m_cellularDistanceIndex1, 0), 3);
	}

	public void SetCellularJitter(float cellularJitter)
	{
		this.m_cellularJitter = cellularJitter;
	}

	public void SetCellularNoiseLookup(FastNoise noise)
	{
		this.m_cellularNoiseLookup = noise;
	}

	public void SetGradientPerturbAmp(float gradientPerturbAmp)
	{
		this.m_gradientPerturbAmp = gradientPerturbAmp;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int FastFloor(float f)
	{
		if (f < 0f)
		{
			return (int)f - 1;
		}
		return (int)f;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int FastRound(float f)
	{
		if (f < 0f)
		{
			return (int)(f - 0.5f);
		}
		return (int)(f + 0.5f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float Lerp(float a, float b, float t)
	{
		return a + t * (b - a);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float InterpHermiteFunc(float t)
	{
		return t * t * (3f - 2f * t);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float InterpQuinticFunc(float t)
	{
		return t * t * t * (t * (t * 6f - 15f) + 10f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float CubicLerp(float a, float b, float c, float d, float t)
	{
		float num = d - c - (a - b);
		return t * t * t * num + t * t * (a - b - num) + t * (c - a) + b;
	}

	private void CalculateFractalBounding()
	{
		float num = this.m_gain;
		float num2 = 1f;
		for (int i = 1; i < this.m_octaves; i++)
		{
			num2 += num;
			num *= this.m_gain;
		}
		this.m_fractalBounding = 1f / num2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int Hash2D(int seed, int x, int y)
	{
		int num = seed ^ 1619 * x;
		num ^= 31337 * y;
		num = num * num * num * 60493;
		return num >> 13 ^ num;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int Hash3D(int seed, int x, int y, int z)
	{
		int num = seed ^ 1619 * x;
		num ^= 31337 * y;
		num ^= 6971 * z;
		num = num * num * num * 60493;
		return num >> 13 ^ num;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int Hash4D(int seed, int x, int y, int z, int w)
	{
		int num = seed ^ 1619 * x;
		num ^= 31337 * y;
		num ^= 6971 * z;
		num ^= 1013 * w;
		num = num * num * num * 60493;
		return num >> 13 ^ num;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float ValCoord2D(int seed, int x, int y)
	{
		int num = seed ^ 1619 * x;
		num ^= 31337 * y;
		return (float)(num * num * num * 60493) / 2.1474836E+09f;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float ValCoord3D(int seed, int x, int y, int z)
	{
		int num = seed ^ 1619 * x;
		num ^= 31337 * y;
		num ^= 6971 * z;
		return (float)(num * num * num * 60493) / 2.1474836E+09f;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float ValCoord4D(int seed, int x, int y, int z, int w)
	{
		int num = seed ^ 1619 * x;
		num ^= 31337 * y;
		num ^= 6971 * z;
		num ^= 1013 * w;
		return (float)(num * num * num * 60493) / 2.1474836E+09f;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float GradCoord2D(int seed, int x, int y, float xd, float yd)
	{
		int num = seed ^ 1619 * x;
		num ^= 31337 * y;
		num = num * num * num * 60493;
		num = (num >> 13 ^ num);
		FastNoise.Float2 @float = FastNoise.GRAD_2D[num & 7];
		return xd * @float.x + yd * @float.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float GradCoord3D(int seed, int x, int y, int z, float xd, float yd, float zd)
	{
		int num = seed ^ 1619 * x;
		num ^= 31337 * y;
		num ^= 6971 * z;
		num = num * num * num * 60493;
		num = (num >> 13 ^ num);
		FastNoise.Float3 @float = FastNoise.GRAD_3D[num & 15];
		return xd * @float.x + yd * @float.y + zd * @float.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float GradCoord4D(int seed, int x, int y, int z, int w, float xd, float yd, float zd, float wd)
	{
		int num = seed ^ 1619 * x;
		num ^= 31337 * y;
		num ^= 6971 * z;
		num ^= 1013 * w;
		num = num * num * num * 60493;
		num = (num >> 13 ^ num);
		num &= 31;
		float num2 = yd;
		float num3 = zd;
		float num4 = wd;
		switch (num >> 3)
		{
			case 1:
				num2 = wd;
				num3 = xd;
				num4 = yd;
				break;
			case 2:
				num2 = zd;
				num3 = wd;
				num4 = xd;
				break;
			case 3:
				num2 = yd;
				num3 = zd;
				num4 = wd;
				break;
		}
		return (((num & 4) == 0) ? (-num2) : num2) + (((num & 2) == 0) ? (-num3) : num3) + (((num & 1) == 0) ? (-num4) : num4);
	}

	public float GetNoise(float x, float y, float z)
	{
		x *= this.m_frequency;
		y *= this.m_frequency;
		z *= this.m_frequency;
		switch (this.m_noiseType)
		{
			case FastNoise.NoiseType.Value:
				return this.SingleValue(this.m_seed, x, y, z);
			case FastNoise.NoiseType.ValueFractal:
				switch (this.m_fractalType)
				{
					case FastNoise.FractalType.FBM:
						return this.SingleValueFractalFBM(x, y, z);
					case FastNoise.FractalType.Billow:
						return this.SingleValueFractalBillow(x, y, z);
					case FastNoise.FractalType.RigidMulti:
						return this.SingleValueFractalRigidMulti(x, y, z);
					default:
						return 0f;
				}
				break;
			case FastNoise.NoiseType.Perlin:
				return this.SinglePerlin(this.m_seed, x, y, z);
			case FastNoise.NoiseType.PerlinFractal:
				switch (this.m_fractalType)
				{
					case FastNoise.FractalType.FBM:
						return this.SinglePerlinFractalFBM(x, y, z);
					case FastNoise.FractalType.Billow:
						return this.SinglePerlinFractalBillow(x, y, z);
					case FastNoise.FractalType.RigidMulti:
						return this.SinglePerlinFractalRigidMulti(x, y, z);
					default:
						return 0f;
				}
				break;
			case FastNoise.NoiseType.Simplex:
				return this.SingleSimplex(this.m_seed, x, y, z);
			case FastNoise.NoiseType.SimplexFractal:
				switch (this.m_fractalType)
				{
					case FastNoise.FractalType.FBM:
						return this.SingleSimplexFractalFBM(x, y, z);
					case FastNoise.FractalType.Billow:
						return this.SingleSimplexFractalBillow(x, y, z);
					case FastNoise.FractalType.RigidMulti:
						return this.SingleSimplexFractalRigidMulti(x, y, z);
					default:
						return 0f;
				}
				break;
			case FastNoise.NoiseType.Cellular:
				{
					FastNoise.CellularReturnType cellularReturnType = this.m_cellularReturnType;
					if (cellularReturnType <= FastNoise.CellularReturnType.Distance)
					{
						return this.SingleCellular(x, y, z);
					}
					return this.SingleCellular2Edge(x, y, z);
				}
			case FastNoise.NoiseType.WhiteNoise:
				return this.GetWhiteNoise(x, y, z);
			case FastNoise.NoiseType.Cubic:
				return this.SingleCubic(this.m_seed, x, y, z);
			case FastNoise.NoiseType.CubicFractal:
				switch (this.m_fractalType)
				{
					case FastNoise.FractalType.FBM:
						return this.SingleCubicFractalFBM(x, y, z);
					case FastNoise.FractalType.Billow:
						return this.SingleCubicFractalBillow(x, y, z);
					case FastNoise.FractalType.RigidMulti:
						return this.SingleCubicFractalRigidMulti(x, y, z);
					default:
						return 0f;
				}
				break;
			default:
				return 0f;
		}
	}

	public float GetNoise(float x, float y)
	{
		x *= this.m_frequency;
		y *= this.m_frequency;
		switch (this.m_noiseType)
		{
			case FastNoise.NoiseType.Value:
				return this.SingleValue(this.m_seed, x, y);
			case FastNoise.NoiseType.ValueFractal:
				switch (this.m_fractalType)
				{
					case FastNoise.FractalType.FBM:
						return this.SingleValueFractalFBM(x, y);
					case FastNoise.FractalType.Billow:
						return this.SingleValueFractalBillow(x, y);
					case FastNoise.FractalType.RigidMulti:
						return this.SingleValueFractalRigidMulti(x, y);
					default:
						return 0f;
				}
				break;
			case FastNoise.NoiseType.Perlin:
				return this.SinglePerlin(this.m_seed, x, y);
			case FastNoise.NoiseType.PerlinFractal:
				switch (this.m_fractalType)
				{
					case FastNoise.FractalType.FBM:
						return this.SinglePerlinFractalFBM(x, y);
					case FastNoise.FractalType.Billow:
						return this.SinglePerlinFractalBillow(x, y);
					case FastNoise.FractalType.RigidMulti:
						return this.SinglePerlinFractalRigidMulti(x, y);
					default:
						return 0f;
				}
				break;
			case FastNoise.NoiseType.Simplex:
				return this.SingleSimplex(this.m_seed, x, y);
			case FastNoise.NoiseType.SimplexFractal:
				switch (this.m_fractalType)
				{
					case FastNoise.FractalType.FBM:
						return this.SingleSimplexFractalFBM(x, y);
					case FastNoise.FractalType.Billow:
						return this.SingleSimplexFractalBillow(x, y);
					case FastNoise.FractalType.RigidMulti:
						return this.SingleSimplexFractalRigidMulti(x, y);
					default:
						return 0f;
				}
				break;
			case FastNoise.NoiseType.Cellular:
				{
					FastNoise.CellularReturnType cellularReturnType = this.m_cellularReturnType;
					if (cellularReturnType <= FastNoise.CellularReturnType.Distance)
					{
						return this.SingleCellular(x, y);
					}
					return this.SingleCellular2Edge(x, y);
				}
			case FastNoise.NoiseType.WhiteNoise:
				return this.GetWhiteNoise(x, y);
			case FastNoise.NoiseType.Cubic:
				return this.SingleCubic(this.m_seed, x, y);
			case FastNoise.NoiseType.CubicFractal:
				switch (this.m_fractalType)
				{
					case FastNoise.FractalType.FBM:
						return this.SingleCubicFractalFBM(x, y);
					case FastNoise.FractalType.Billow:
						return this.SingleCubicFractalBillow(x, y);
					case FastNoise.FractalType.RigidMulti:
						return this.SingleCubicFractalRigidMulti(x, y);
					default:
						return 0f;
				}
				break;
			default:
				return 0f;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private int FloatCast2Int(float f)
	{
		long num = BitConverter.DoubleToInt64Bits((double)f);
		return (int)(num ^ num >> 32);
	}

	public float GetWhiteNoise(float x, float y, float z, float w)
	{
		int x2 = this.FloatCast2Int(x);
		int y2 = this.FloatCast2Int(y);
		int z2 = this.FloatCast2Int(z);
		int w2 = this.FloatCast2Int(w);
		return FastNoise.ValCoord4D(this.m_seed, x2, y2, z2, w2);
	}

	public float GetWhiteNoise(float x, float y, float z)
	{
		int x2 = this.FloatCast2Int(x);
		int y2 = this.FloatCast2Int(y);
		int z2 = this.FloatCast2Int(z);
		return FastNoise.ValCoord3D(this.m_seed, x2, y2, z2);
	}

	public float GetWhiteNoise(float x, float y)
	{
		int x2 = this.FloatCast2Int(x);
		int y2 = this.FloatCast2Int(y);
		return FastNoise.ValCoord2D(this.m_seed, x2, y2);
	}

	public float GetWhiteNoiseInt(int x, int y, int z, int w)
	{
		return FastNoise.ValCoord4D(this.m_seed, x, y, z, w);
	}

	public float GetWhiteNoiseInt(int x, int y, int z)
	{
		return FastNoise.ValCoord3D(this.m_seed, x, y, z);
	}

	public float GetWhiteNoiseInt(int x, int y)
	{
		return FastNoise.ValCoord2D(this.m_seed, x, y);
	}

	public float GetValueFractal(float x, float y, float z)
	{
		x *= this.m_frequency;
		y *= this.m_frequency;
		z *= this.m_frequency;
		switch (this.m_fractalType)
		{
			case FastNoise.FractalType.FBM:
				return this.SingleValueFractalFBM(x, y, z);
			case FastNoise.FractalType.Billow:
				return this.SingleValueFractalBillow(x, y, z);
			case FastNoise.FractalType.RigidMulti:
				return this.SingleValueFractalRigidMulti(x, y, z);
			default:
				return 0f;
		}
	}

	private float SingleValueFractalFBM(float x, float y, float z)
	{
		int num = this.m_seed;
		float num2 = this.SingleValue(num, x, y, z);
		float num3 = 1f;
		for (int i = 1; i < this.m_octaves; i++)
		{
			x *= this.m_lacunarity;
			y *= this.m_lacunarity;
			z *= this.m_lacunarity;
			num3 *= this.m_gain;
			num2 += this.SingleValue(++num, x, y, z) * num3;
		}
		return num2 * this.m_fractalBounding;
	}

	private float SingleValueFractalBillow(float x, float y, float z)
	{
		int num = this.m_seed;
		float num2 = Math.Abs(this.SingleValue(num, x, y, z)) * 2f - 1f;
		float num3 = 1f;
		for (int i = 1; i < this.m_octaves; i++)
		{
			x *= this.m_lacunarity;
			y *= this.m_lacunarity;
			z *= this.m_lacunarity;
			num3 *= this.m_gain;
			num2 += (Math.Abs(this.SingleValue(++num, x, y, z)) * 2f - 1f) * num3;
		}
		return num2 * this.m_fractalBounding;
	}

	private float SingleValueFractalRigidMulti(float x, float y, float z)
	{
		int num = this.m_seed;
		float num2 = 1f - Math.Abs(this.SingleValue(num, x, y, z));
		float num3 = 1f;
		for (int i = 1; i < this.m_octaves; i++)
		{
			x *= this.m_lacunarity;
			y *= this.m_lacunarity;
			z *= this.m_lacunarity;
			num3 *= this.m_gain;
			num2 -= (1f - Math.Abs(this.SingleValue(++num, x, y, z))) * num3;
		}
		return num2;
	}

	public float GetValue(float x, float y, float z)
	{
		return this.SingleValue(this.m_seed, x * this.m_frequency, y * this.m_frequency, z * this.m_frequency);
	}

	private float SingleValue(int seed, float x, float y, float z)
	{
		int num = FastNoise.FastFloor(x);
		int num2 = FastNoise.FastFloor(y);
		int num3 = FastNoise.FastFloor(z);
		int x2 = num + 1;
		int y2 = num2 + 1;
		int z2 = num3 + 1;
		float t;
		float t2;
		float t3;
		switch (this.m_interp)
		{
			default:
				t = x - (float)num;
				t2 = y - (float)num2;
				t3 = z - (float)num3;
				break;
			case FastNoise.Interp.Hermite:
				t = FastNoise.InterpHermiteFunc(x - (float)num);
				t2 = FastNoise.InterpHermiteFunc(y - (float)num2);
				t3 = FastNoise.InterpHermiteFunc(z - (float)num3);
				break;
			case FastNoise.Interp.Quintic:
				t = FastNoise.InterpQuinticFunc(x - (float)num);
				t2 = FastNoise.InterpQuinticFunc(y - (float)num2);
				t3 = FastNoise.InterpQuinticFunc(z - (float)num3);
				break;
		}
		float a = FastNoise.Lerp(FastNoise.ValCoord3D(seed, num, num2, num3), FastNoise.ValCoord3D(seed, x2, num2, num3), t);
		float b = FastNoise.Lerp(FastNoise.ValCoord3D(seed, num, y2, num3), FastNoise.ValCoord3D(seed, x2, y2, num3), t);
		float a2 = FastNoise.Lerp(FastNoise.ValCoord3D(seed, num, num2, z2), FastNoise.ValCoord3D(seed, x2, num2, z2), t);
		float b2 = FastNoise.Lerp(FastNoise.ValCoord3D(seed, num, y2, z2), FastNoise.ValCoord3D(seed, x2, y2, z2), t);
		float a3 = FastNoise.Lerp(a, b, t2);
		float b3 = FastNoise.Lerp(a2, b2, t2);
		return FastNoise.Lerp(a3, b3, t3);
	}

	public float GetValueFractal(float x, float y)
	{
		x *= this.m_frequency;
		y *= this.m_frequency;
		switch (this.m_fractalType)
		{
			case FastNoise.FractalType.FBM:
				return this.SingleValueFractalFBM(x, y);
			case FastNoise.FractalType.Billow:
				return this.SingleValueFractalBillow(x, y);
			case FastNoise.FractalType.RigidMulti:
				return this.SingleValueFractalRigidMulti(x, y);
			default:
				return 0f;
		}
	}

	private float SingleValueFractalFBM(float x, float y)
	{
		int num = this.m_seed;
		float num2 = this.SingleValue(num, x, y);
		float num3 = 1f;
		for (int i = 1; i < this.m_octaves; i++)
		{
			x *= this.m_lacunarity;
			y *= this.m_lacunarity;
			num3 *= this.m_gain;
			num2 += this.SingleValue(++num, x, y) * num3;
		}
		return num2 * this.m_fractalBounding;
	}

	private float SingleValueFractalBillow(float x, float y)
	{
		int num = this.m_seed;
		float num2 = Math.Abs(this.SingleValue(num, x, y)) * 2f - 1f;
		float num3 = 1f;
		for (int i = 1; i < this.m_octaves; i++)
		{
			x *= this.m_lacunarity;
			y *= this.m_lacunarity;
			num3 *= this.m_gain;
			num2 += (Math.Abs(this.SingleValue(++num, x, y)) * 2f - 1f) * num3;
		}
		return num2 * this.m_fractalBounding;
	}

	private float SingleValueFractalRigidMulti(float x, float y)
	{
		int num = this.m_seed;
		float num2 = 1f - Math.Abs(this.SingleValue(num, x, y));
		float num3 = 1f;
		for (int i = 1; i < this.m_octaves; i++)
		{
			x *= this.m_lacunarity;
			y *= this.m_lacunarity;
			num3 *= this.m_gain;
			num2 -= (1f - Math.Abs(this.SingleValue(++num, x, y))) * num3;
		}
		return num2;
	}

	public float GetValue(float x, float y)
	{
		return this.SingleValue(this.m_seed, x * this.m_frequency, y * this.m_frequency);
	}

	private float SingleValue(int seed, float x, float y)
	{
		int num = FastNoise.FastFloor(x);
		int num2 = FastNoise.FastFloor(y);
		int x2 = num + 1;
		int y2 = num2 + 1;
		float t;
		float t2;
		switch (this.m_interp)
		{
			default:
				t = x - (float)num;
				t2 = y - (float)num2;
				break;
			case FastNoise.Interp.Hermite:
				t = FastNoise.InterpHermiteFunc(x - (float)num);
				t2 = FastNoise.InterpHermiteFunc(y - (float)num2);
				break;
			case FastNoise.Interp.Quintic:
				t = FastNoise.InterpQuinticFunc(x - (float)num);
				t2 = FastNoise.InterpQuinticFunc(y - (float)num2);
				break;
		}
		float a = FastNoise.Lerp(FastNoise.ValCoord2D(seed, num, num2), FastNoise.ValCoord2D(seed, x2, num2), t);
		float b = FastNoise.Lerp(FastNoise.ValCoord2D(seed, num, y2), FastNoise.ValCoord2D(seed, x2, y2), t);
		return FastNoise.Lerp(a, b, t2);
	}

	public float GetPerlinFractal(float x, float y, float z)
	{
		x *= this.m_frequency;
		y *= this.m_frequency;
		z *= this.m_frequency;
		switch (this.m_fractalType)
		{
			case FastNoise.FractalType.FBM:
				return this.SinglePerlinFractalFBM(x, y, z);
			case FastNoise.FractalType.Billow:
				return this.SinglePerlinFractalBillow(x, y, z);
			case FastNoise.FractalType.RigidMulti:
				return this.SinglePerlinFractalRigidMulti(x, y, z);
			default:
				return 0f;
		}
	}

	private float SinglePerlinFractalFBM(float x, float y, float z)
	{
		int num = this.m_seed;
		float num2 = this.SinglePerlin(num, x, y, z);
		float num3 = 1f;
		for (int i = 1; i < this.m_octaves; i++)
		{
			x *= this.m_lacunarity;
			y *= this.m_lacunarity;
			z *= this.m_lacunarity;
			num3 *= this.m_gain;
			num2 += this.SinglePerlin(++num, x, y, z) * num3;
		}
		return num2 * this.m_fractalBounding;
	}

	private float SinglePerlinFractalBillow(float x, float y, float z)
	{
		int num = this.m_seed;
		float num2 = Math.Abs(this.SinglePerlin(num, x, y, z)) * 2f - 1f;
		float num3 = 1f;
		for (int i = 1; i < this.m_octaves; i++)
		{
			x *= this.m_lacunarity;
			y *= this.m_lacunarity;
			z *= this.m_lacunarity;
			num3 *= this.m_gain;
			num2 += (Math.Abs(this.SinglePerlin(++num, x, y, z)) * 2f - 1f) * num3;
		}
		return num2 * this.m_fractalBounding;
	}

	private float SinglePerlinFractalRigidMulti(float x, float y, float z)
	{
		int num = this.m_seed;
		float num2 = 1f - Math.Abs(this.SinglePerlin(num, x, y, z));
		float num3 = 1f;
		for (int i = 1; i < this.m_octaves; i++)
		{
			x *= this.m_lacunarity;
			y *= this.m_lacunarity;
			z *= this.m_lacunarity;
			num3 *= this.m_gain;
			num2 -= (1f - Math.Abs(this.SinglePerlin(++num, x, y, z))) * num3;
		}
		return num2;
	}

	public float GetPerlin(float x, float y, float z)
	{
		return this.SinglePerlin(this.m_seed, x * this.m_frequency, y * this.m_frequency, z * this.m_frequency);
	}

	private float SinglePerlin(int seed, float x, float y, float z)
	{
		int num = FastNoise.FastFloor(x);
		int num2 = FastNoise.FastFloor(y);
		int num3 = FastNoise.FastFloor(z);
		int x2 = num + 1;
		int y2 = num2 + 1;
		int z2 = num3 + 1;
		float t;
		float t2;
		float t3;
		switch (this.m_interp)
		{
			default:
				t = x - (float)num;
				t2 = y - (float)num2;
				t3 = z - (float)num3;
				break;
			case FastNoise.Interp.Hermite:
				t = FastNoise.InterpHermiteFunc(x - (float)num);
				t2 = FastNoise.InterpHermiteFunc(y - (float)num2);
				t3 = FastNoise.InterpHermiteFunc(z - (float)num3);
				break;
			case FastNoise.Interp.Quintic:
				t = FastNoise.InterpQuinticFunc(x - (float)num);
				t2 = FastNoise.InterpQuinticFunc(y - (float)num2);
				t3 = FastNoise.InterpQuinticFunc(z - (float)num3);
				break;
		}
		float num4 = x - (float)num;
		float num5 = y - (float)num2;
		float num6 = z - (float)num3;
		float xd = num4 - 1f;
		float yd = num5 - 1f;
		float zd = num6 - 1f;
		float a = FastNoise.Lerp(FastNoise.GradCoord3D(seed, num, num2, num3, num4, num5, num6), FastNoise.GradCoord3D(seed, x2, num2, num3, xd, num5, num6), t);
		float b = FastNoise.Lerp(FastNoise.GradCoord3D(seed, num, y2, num3, num4, yd, num6), FastNoise.GradCoord3D(seed, x2, y2, num3, xd, yd, num6), t);
		float a2 = FastNoise.Lerp(FastNoise.GradCoord3D(seed, num, num2, z2, num4, num5, zd), FastNoise.GradCoord3D(seed, x2, num2, z2, xd, num5, zd), t);
		float b2 = FastNoise.Lerp(FastNoise.GradCoord3D(seed, num, y2, z2, num4, yd, zd), FastNoise.GradCoord3D(seed, x2, y2, z2, xd, yd, zd), t);
		float a3 = FastNoise.Lerp(a, b, t2);
		float b3 = FastNoise.Lerp(a2, b2, t2);
		return FastNoise.Lerp(a3, b3, t3);
	}

	public float GetPerlinFractal(float x, float y)
	{
		x *= this.m_frequency;
		y *= this.m_frequency;
		switch (this.m_fractalType)
		{
			case FastNoise.FractalType.FBM:
				return this.SinglePerlinFractalFBM(x, y);
			case FastNoise.FractalType.Billow:
				return this.SinglePerlinFractalBillow(x, y);
			case FastNoise.FractalType.RigidMulti:
				return this.SinglePerlinFractalRigidMulti(x, y);
			default:
				return 0f;
		}
	}

	private float SinglePerlinFractalFBM(float x, float y)
	{
		int num = this.m_seed;
		float num2 = this.SinglePerlin(num, x, y);
		float num3 = 1f;
		for (int i = 1; i < this.m_octaves; i++)
		{
			x *= this.m_lacunarity;
			y *= this.m_lacunarity;
			num3 *= this.m_gain;
			num2 += this.SinglePerlin(++num, x, y) * num3;
		}
		return num2 * this.m_fractalBounding;
	}

	private float SinglePerlinFractalBillow(float x, float y)
	{
		int num = this.m_seed;
		float num2 = Math.Abs(this.SinglePerlin(num, x, y)) * 2f - 1f;
		float num3 = 1f;
		for (int i = 1; i < this.m_octaves; i++)
		{
			x *= this.m_lacunarity;
			y *= this.m_lacunarity;
			num3 *= this.m_gain;
			num2 += (Math.Abs(this.SinglePerlin(++num, x, y)) * 2f - 1f) * num3;
		}
		return num2 * this.m_fractalBounding;
	}

	private float SinglePerlinFractalRigidMulti(float x, float y)
	{
		int num = this.m_seed;
		float num2 = 1f - Math.Abs(this.SinglePerlin(num, x, y));
		float num3 = 1f;
		for (int i = 1; i < this.m_octaves; i++)
		{
			x *= this.m_lacunarity;
			y *= this.m_lacunarity;
			num3 *= this.m_gain;
			num2 -= (1f - Math.Abs(this.SinglePerlin(++num, x, y))) * num3;
		}
		return num2;
	}

	public float GetPerlin(float x, float y)
	{
		return this.SinglePerlin(this.m_seed, x * this.m_frequency, y * this.m_frequency);
	}

	private float SinglePerlin(int seed, float x, float y)
	{
		int num = FastNoise.FastFloor(x);
		int num2 = FastNoise.FastFloor(y);
		int x2 = num + 1;
		int y2 = num2 + 1;
		float t;
		float t2;
		switch (this.m_interp)
		{
			default:
				t = x - (float)num;
				t2 = y - (float)num2;
				break;
			case FastNoise.Interp.Hermite:
				t = FastNoise.InterpHermiteFunc(x - (float)num);
				t2 = FastNoise.InterpHermiteFunc(y - (float)num2);
				break;
			case FastNoise.Interp.Quintic:
				t = FastNoise.InterpQuinticFunc(x - (float)num);
				t2 = FastNoise.InterpQuinticFunc(y - (float)num2);
				break;
		}
		float num3 = x - (float)num;
		float num4 = y - (float)num2;
		float xd = num3 - 1f;
		float yd = num4 - 1f;
		float a = FastNoise.Lerp(FastNoise.GradCoord2D(seed, num, num2, num3, num4), FastNoise.GradCoord2D(seed, x2, num2, xd, num4), t);
		float b = FastNoise.Lerp(FastNoise.GradCoord2D(seed, num, y2, num3, yd), FastNoise.GradCoord2D(seed, x2, y2, xd, yd), t);
		return FastNoise.Lerp(a, b, t2);
	}

	public float GetSimplexFractal(float x, float y, float z)
	{
		x *= this.m_frequency;
		y *= this.m_frequency;
		z *= this.m_frequency;
		switch (this.m_fractalType)
		{
			case FastNoise.FractalType.FBM:
				return this.SingleSimplexFractalFBM(x, y, z);
			case FastNoise.FractalType.Billow:
				return this.SingleSimplexFractalBillow(x, y, z);
			case FastNoise.FractalType.RigidMulti:
				return this.SingleSimplexFractalRigidMulti(x, y, z);
			default:
				return 0f;
		}
	}

	private float SingleSimplexFractalFBM(float x, float y, float z)
	{
		int num = this.m_seed;
		float num2 = this.SingleSimplex(num, x, y, z);
		float num3 = 1f;
		for (int i = 1; i < this.m_octaves; i++)
		{
			x *= this.m_lacunarity;
			y *= this.m_lacunarity;
			z *= this.m_lacunarity;
			num3 *= this.m_gain;
			num2 += this.SingleSimplex(++num, x, y, z) * num3;
		}
		return num2 * this.m_fractalBounding;
	}

	private float SingleSimplexFractalBillow(float x, float y, float z)
	{
		int num = this.m_seed;
		float num2 = Math.Abs(this.SingleSimplex(num, x, y, z)) * 2f - 1f;
		float num3 = 1f;
		for (int i = 1; i < this.m_octaves; i++)
		{
			x *= this.m_lacunarity;
			y *= this.m_lacunarity;
			z *= this.m_lacunarity;
			num3 *= this.m_gain;
			num2 += (Math.Abs(this.SingleSimplex(++num, x, y, z)) * 2f - 1f) * num3;
		}
		return num2 * this.m_fractalBounding;
	}

	private float SingleSimplexFractalRigidMulti(float x, float y, float z)
	{
		int num = this.m_seed;
		float num2 = 1f - Math.Abs(this.SingleSimplex(num, x, y, z));
		float num3 = 1f;
		for (int i = 1; i < this.m_octaves; i++)
		{
			x *= this.m_lacunarity;
			y *= this.m_lacunarity;
			z *= this.m_lacunarity;
			num3 *= this.m_gain;
			num2 -= (1f - Math.Abs(this.SingleSimplex(++num, x, y, z))) * num3;
		}
		return num2;
	}

	public float GetSimplex(float x, float y, float z)
	{
		return this.SingleSimplex(this.m_seed, x * this.m_frequency, y * this.m_frequency, z * this.m_frequency);
	}

	private float SingleSimplex(int seed, float x, float y, float z)
	{
		float num = (x + y + z) * 0.33333334f;
		int num2 = FastNoise.FastFloor(x + num);
		int num3 = FastNoise.FastFloor(y + num);
		int num4 = FastNoise.FastFloor(z + num);
		num = (float)(num2 + num3 + num4) * 0.16666667f;
		float num5 = x - ((float)num2 - num);
		float num6 = y - ((float)num3 - num);
		float num7 = z - ((float)num4 - num);
		int num8;
		int num9;
		int num10;
		int num11;
		int num12;
		int num13;
		if (num5 >= num6)
		{
			if (num6 >= num7)
			{
				num8 = 1;
				num9 = 0;
				num10 = 0;
				num11 = 1;
				num12 = 1;
				num13 = 0;
			}
			else if (num5 >= num7)
			{
				num8 = 1;
				num9 = 0;
				num10 = 0;
				num11 = 1;
				num12 = 0;
				num13 = 1;
			}
			else
			{
				num8 = 0;
				num9 = 0;
				num10 = 1;
				num11 = 1;
				num12 = 0;
				num13 = 1;
			}
		}
		else if (num6 < num7)
		{
			num8 = 0;
			num9 = 0;
			num10 = 1;
			num11 = 0;
			num12 = 1;
			num13 = 1;
		}
		else if (num5 < num7)
		{
			num8 = 0;
			num9 = 1;
			num10 = 0;
			num11 = 0;
			num12 = 1;
			num13 = 1;
		}
		else
		{
			num8 = 0;
			num9 = 1;
			num10 = 0;
			num11 = 1;
			num12 = 1;
			num13 = 0;
		}
		float num14 = num5 - (float)num8 + 0.16666667f;
		float num15 = num6 - (float)num9 + 0.16666667f;
		float num16 = num7 - (float)num10 + 0.16666667f;
		float num17 = num5 - (float)num11 + 0.33333334f;
		float num18 = num6 - (float)num12 + 0.33333334f;
		float num19 = num7 - (float)num13 + 0.33333334f;
		float num20 = num5 + -0.5f;
		float num21 = num6 + -0.5f;
		float num22 = num7 + -0.5f;
		num = 0.6f - num5 * num5 - num6 * num6 - num7 * num7;
		float num23;
		if (num < 0f)
		{
			num23 = 0f;
		}
		else
		{
			num *= num;
			num23 = num * num * FastNoise.GradCoord3D(seed, num2, num3, num4, num5, num6, num7);
		}
		num = 0.6f - num14 * num14 - num15 * num15 - num16 * num16;
		float num24;
		if (num < 0f)
		{
			num24 = 0f;
		}
		else
		{
			num *= num;
			num24 = num * num * FastNoise.GradCoord3D(seed, num2 + num8, num3 + num9, num4 + num10, num14, num15, num16);
		}
		num = 0.6f - num17 * num17 - num18 * num18 - num19 * num19;
		float num25;
		if (num < 0f)
		{
			num25 = 0f;
		}
		else
		{
			num *= num;
			num25 = num * num * FastNoise.GradCoord3D(seed, num2 + num11, num3 + num12, num4 + num13, num17, num18, num19);
		}
		num = 0.6f - num20 * num20 - num21 * num21 - num22 * num22;
		float num26;
		if (num < 0f)
		{
			num26 = 0f;
		}
		else
		{
			num *= num;
			num26 = num * num * FastNoise.GradCoord3D(seed, num2 + 1, num3 + 1, num4 + 1, num20, num21, num22);
		}
		return 32f * (num23 + num24 + num25 + num26);
	}

	public float GetSimplexFractal(float x, float y)
	{
		x *= this.m_frequency;
		y *= this.m_frequency;
		switch (this.m_fractalType)
		{
			case FastNoise.FractalType.FBM:
				return this.SingleSimplexFractalFBM(x, y);
			case FastNoise.FractalType.Billow:
				return this.SingleSimplexFractalBillow(x, y);
			case FastNoise.FractalType.RigidMulti:
				return this.SingleSimplexFractalRigidMulti(x, y);
			default:
				return 0f;
		}
	}

	private float SingleSimplexFractalFBM(float x, float y)
	{
		int num = this.m_seed;
		float num2 = this.SingleSimplex(num, x, y);
		float num3 = 1f;
		for (int i = 1; i < this.m_octaves; i++)
		{
			x *= this.m_lacunarity;
			y *= this.m_lacunarity;
			num3 *= this.m_gain;
			num2 += this.SingleSimplex(++num, x, y) * num3;
		}
		return num2 * this.m_fractalBounding;
	}

	private float SingleSimplexFractalBillow(float x, float y)
	{
		int num = this.m_seed;
		float num2 = Math.Abs(this.SingleSimplex(num, x, y)) * 2f - 1f;
		float num3 = 1f;
		for (int i = 1; i < this.m_octaves; i++)
		{
			x *= this.m_lacunarity;
			y *= this.m_lacunarity;
			num3 *= this.m_gain;
			num2 += (Math.Abs(this.SingleSimplex(++num, x, y)) * 2f - 1f) * num3;
		}
		return num2 * this.m_fractalBounding;
	}

	private float SingleSimplexFractalRigidMulti(float x, float y)
	{
		int num = this.m_seed;
		float num2 = 1f - Math.Abs(this.SingleSimplex(num, x, y));
		float num3 = 1f;
		for (int i = 1; i < this.m_octaves; i++)
		{
			x *= this.m_lacunarity;
			y *= this.m_lacunarity;
			num3 *= this.m_gain;
			num2 -= (1f - Math.Abs(this.SingleSimplex(++num, x, y))) * num3;
		}
		return num2;
	}

	public float GetSimplex(float x, float y)
	{
		return this.SingleSimplex(this.m_seed, x * this.m_frequency, y * this.m_frequency);
	}

	private float SingleSimplex(int seed, float x, float y)
	{
		float num = (x + y) * 0.3660254f;
		int num2 = FastNoise.FastFloor(x + num);
		int num3 = FastNoise.FastFloor(y + num);
		num = (float)(num2 + num3) * 0.21132487f;
		float num4 = (float)num2 - num;
		float num5 = (float)num3 - num;
		float num6 = x - num4;
		float num7 = y - num5;
		int num8;
		int num9;
		if (num6 > num7)
		{
			num8 = 1;
			num9 = 0;
		}
		else
		{
			num8 = 0;
			num9 = 1;
		}
		float num10 = num6 - (float)num8 + 0.21132487f;
		float num11 = num7 - (float)num9 + 0.21132487f;
		float num12 = num6 - 1f + 0.42264974f;
		float num13 = num7 - 1f + 0.42264974f;
		num = 0.5f - num6 * num6 - num7 * num7;
		float num14;
		if (num < 0f)
		{
			num14 = 0f;
		}
		else
		{
			num *= num;
			num14 = num * num * FastNoise.GradCoord2D(seed, num2, num3, num6, num7);
		}
		num = 0.5f - num10 * num10 - num11 * num11;
		float num15;
		if (num < 0f)
		{
			num15 = 0f;
		}
		else
		{
			num *= num;
			num15 = num * num * FastNoise.GradCoord2D(seed, num2 + num8, num3 + num9, num10, num11);
		}
		num = 0.5f - num12 * num12 - num13 * num13;
		float num16;
		if (num < 0f)
		{
			num16 = 0f;
		}
		else
		{
			num *= num;
			num16 = num * num * FastNoise.GradCoord2D(seed, num2 + 1, num3 + 1, num12, num13);
		}
		return 50f * (num14 + num15 + num16);
	}

	public float GetSimplex(float x, float y, float z, float w)
	{
		return this.SingleSimplex(this.m_seed, x * this.m_frequency, y * this.m_frequency, z * this.m_frequency, w * this.m_frequency);
	}

	private float SingleSimplex(int seed, float x, float y, float z, float w)
	{
		float num = (x + y + z + w) * 0.309017f;
		int num2 = FastNoise.FastFloor(x + num);
		int num3 = FastNoise.FastFloor(y + num);
		int num4 = FastNoise.FastFloor(z + num);
		int num5 = FastNoise.FastFloor(w + num);
		num = (float)(num2 + num3 + num4 + num5) * 0.1381966f;
		float num6 = (float)num2 - num;
		float num7 = (float)num3 - num;
		float num8 = (float)num4 - num;
		float num9 = (float)num5 - num;
		float num10 = x - num6;
		float num11 = y - num7;
		float num12 = z - num8;
		float num13 = w - num9;
		int num14 = (num10 > num11) ? 32 : 0;
		num14 += ((num10 > num12) ? 16 : 0);
		num14 += ((num11 > num12) ? 8 : 0);
		num14 += ((num10 > num13) ? 4 : 0);
		num14 += ((num11 > num13) ? 2 : 0);
		num14 += ((num12 > num13) ? 1 : 0);
		num14 <<= 2;
		int num15 = (FastNoise.SIMPLEX_4D[num14] >= 3) ? 1 : 0;
		int num16 = (FastNoise.SIMPLEX_4D[num14] >= 2) ? 1 : 0;
		int num17 = (FastNoise.SIMPLEX_4D[num14++] >= 1) ? 1 : 0;
		int num18 = (FastNoise.SIMPLEX_4D[num14] >= 3) ? 1 : 0;
		int num19 = (FastNoise.SIMPLEX_4D[num14] >= 2) ? 1 : 0;
		int num20 = (FastNoise.SIMPLEX_4D[num14++] >= 1) ? 1 : 0;
		int num21 = (FastNoise.SIMPLEX_4D[num14] >= 3) ? 1 : 0;
		int num22 = (FastNoise.SIMPLEX_4D[num14] >= 2) ? 1 : 0;
		int num23 = (FastNoise.SIMPLEX_4D[num14++] >= 1) ? 1 : 0;
		int num24 = (FastNoise.SIMPLEX_4D[num14] >= 3) ? 1 : 0;
		int num25 = (FastNoise.SIMPLEX_4D[num14] >= 2) ? 1 : 0;
		int num26 = (FastNoise.SIMPLEX_4D[num14] >= 1) ? 1 : 0;
		float num27 = num10 - (float)num15 + 0.1381966f;
		float num28 = num11 - (float)num18 + 0.1381966f;
		float num29 = num12 - (float)num21 + 0.1381966f;
		float num30 = num13 - (float)num24 + 0.1381966f;
		float num31 = num10 - (float)num16 + 0.2763932f;
		float num32 = num11 - (float)num19 + 0.2763932f;
		float num33 = num12 - (float)num22 + 0.2763932f;
		float num34 = num13 - (float)num25 + 0.2763932f;
		float num35 = num10 - (float)num17 + 0.41458982f;
		float num36 = num11 - (float)num20 + 0.41458982f;
		float num37 = num12 - (float)num23 + 0.41458982f;
		float num38 = num13 - (float)num26 + 0.41458982f;
		float num39 = num10 - 1f + 0.5527864f;
		float num40 = num11 - 1f + 0.5527864f;
		float num41 = num12 - 1f + 0.5527864f;
		float num42 = num13 - 1f + 0.5527864f;
		num = 0.6f - num10 * num10 - num11 * num11 - num12 * num12 - num13 * num13;
		float num43;
		if (num < 0f)
		{
			num43 = 0f;
		}
		else
		{
			num *= num;
			num43 = num * num * FastNoise.GradCoord4D(seed, num2, num3, num4, num5, num10, num11, num12, num13);
		}
		num = 0.6f - num27 * num27 - num28 * num28 - num29 * num29 - num30 * num30;
		float num44;
		if (num < 0f)
		{
			num44 = 0f;
		}
		else
		{
			num *= num;
			num44 = num * num * FastNoise.GradCoord4D(seed, num2 + num15, num3 + num18, num4 + num21, num5 + num24, num27, num28, num29, num30);
		}
		num = 0.6f - num31 * num31 - num32 * num32 - num33 * num33 - num34 * num34;
		float num45;
		if (num < 0f)
		{
			num45 = 0f;
		}
		else
		{
			num *= num;
			num45 = num * num * FastNoise.GradCoord4D(seed, num2 + num16, num3 + num19, num4 + num22, num5 + num25, num31, num32, num33, num34);
		}
		num = 0.6f - num35 * num35 - num36 * num36 - num37 * num37 - num38 * num38;
		float num46;
		if (num < 0f)
		{
			num46 = 0f;
		}
		else
		{
			num *= num;
			num46 = num * num * FastNoise.GradCoord4D(seed, num2 + num17, num3 + num20, num4 + num23, num5 + num26, num35, num36, num37, num38);
		}
		num = 0.6f - num39 * num39 - num40 * num40 - num41 * num41 - num42 * num42;
		float num47;
		if (num < 0f)
		{
			num47 = 0f;
		}
		else
		{
			num *= num;
			num47 = num * num * FastNoise.GradCoord4D(seed, num2 + 1, num3 + 1, num4 + 1, num5 + 1, num39, num40, num41, num42);
		}
		return 27f * (num43 + num44 + num45 + num46 + num47);
	}

	public float GetCubicFractal(float x, float y, float z)
	{
		x *= this.m_frequency;
		y *= this.m_frequency;
		z *= this.m_frequency;
		switch (this.m_fractalType)
		{
			case FastNoise.FractalType.FBM:
				return this.SingleCubicFractalFBM(x, y, z);
			case FastNoise.FractalType.Billow:
				return this.SingleCubicFractalBillow(x, y, z);
			case FastNoise.FractalType.RigidMulti:
				return this.SingleCubicFractalRigidMulti(x, y, z);
			default:
				return 0f;
		}
	}

	private float SingleCubicFractalFBM(float x, float y, float z)
	{
		int num = this.m_seed;
		float num2 = this.SingleCubic(num, x, y, z);
		float num3 = 1f;
		int num4 = 0;
		while (++num4 < this.m_octaves)
		{
			x *= this.m_lacunarity;
			y *= this.m_lacunarity;
			z *= this.m_lacunarity;
			num3 *= this.m_gain;
			num2 += this.SingleCubic(++num, x, y, z) * num3;
		}
		return num2 * this.m_fractalBounding;
	}

	private float SingleCubicFractalBillow(float x, float y, float z)
	{
		int num = this.m_seed;
		float num2 = Math.Abs(this.SingleCubic(num, x, y, z)) * 2f - 1f;
		float num3 = 1f;
		int num4 = 0;
		while (++num4 < this.m_octaves)
		{
			x *= this.m_lacunarity;
			y *= this.m_lacunarity;
			z *= this.m_lacunarity;
			num3 *= this.m_gain;
			num2 += (Math.Abs(this.SingleCubic(++num, x, y, z)) * 2f - 1f) * num3;
		}
		return num2 * this.m_fractalBounding;
	}

	private float SingleCubicFractalRigidMulti(float x, float y, float z)
	{
		int num = this.m_seed;
		float num2 = 1f - Math.Abs(this.SingleCubic(num, x, y, z));
		float num3 = 1f;
		int num4 = 0;
		while (++num4 < this.m_octaves)
		{
			x *= this.m_lacunarity;
			y *= this.m_lacunarity;
			z *= this.m_lacunarity;
			num3 *= this.m_gain;
			num2 -= (1f - Math.Abs(this.SingleCubic(++num, x, y, z))) * num3;
		}
		return num2;
	}

	public float GetCubic(float x, float y, float z)
	{
		return this.SingleCubic(this.m_seed, x * this.m_frequency, y * this.m_frequency, z * this.m_frequency);
	}

	private float SingleCubic(int seed, float x, float y, float z)
	{
		int num = FastNoise.FastFloor(x);
		int num2 = FastNoise.FastFloor(y);
		int num3 = FastNoise.FastFloor(z);
		int x2 = num - 1;
		int y2 = num2 - 1;
		int z2 = num3 - 1;
		int x3 = num + 1;
		int y3 = num2 + 1;
		int z3 = num3 + 1;
		int x4 = num + 2;
		int y4 = num2 + 2;
		int z4 = num3 + 2;
		float t = x - (float)num;
		float t2 = y - (float)num2;
		float t3 = z - (float)num3;
		return FastNoise.CubicLerp(FastNoise.CubicLerp(FastNoise.CubicLerp(FastNoise.ValCoord3D(seed, x2, y2, z2), FastNoise.ValCoord3D(seed, num, y2, z2), FastNoise.ValCoord3D(seed, x3, y2, z2), FastNoise.ValCoord3D(seed, x4, y2, z2), t), FastNoise.CubicLerp(FastNoise.ValCoord3D(seed, x2, num2, z2), FastNoise.ValCoord3D(seed, num, num2, z2), FastNoise.ValCoord3D(seed, x3, num2, z2), FastNoise.ValCoord3D(seed, x4, num2, z2), t), FastNoise.CubicLerp(FastNoise.ValCoord3D(seed, x2, y3, z2), FastNoise.ValCoord3D(seed, num, y3, z2), FastNoise.ValCoord3D(seed, x3, y3, z2), FastNoise.ValCoord3D(seed, x4, y3, z2), t), FastNoise.CubicLerp(FastNoise.ValCoord3D(seed, x2, y4, z2), FastNoise.ValCoord3D(seed, num, y4, z2), FastNoise.ValCoord3D(seed, x3, y4, z2), FastNoise.ValCoord3D(seed, x4, y4, z2), t), t2), FastNoise.CubicLerp(FastNoise.CubicLerp(FastNoise.ValCoord3D(seed, x2, y2, num3), FastNoise.ValCoord3D(seed, num, y2, num3), FastNoise.ValCoord3D(seed, x3, y2, num3), FastNoise.ValCoord3D(seed, x4, y2, num3), t), FastNoise.CubicLerp(FastNoise.ValCoord3D(seed, x2, num2, num3), FastNoise.ValCoord3D(seed, num, num2, num3), FastNoise.ValCoord3D(seed, x3, num2, num3), FastNoise.ValCoord3D(seed, x4, num2, num3), t), FastNoise.CubicLerp(FastNoise.ValCoord3D(seed, x2, y3, num3), FastNoise.ValCoord3D(seed, num, y3, num3), FastNoise.ValCoord3D(seed, x3, y3, num3), FastNoise.ValCoord3D(seed, x4, y3, num3), t), FastNoise.CubicLerp(FastNoise.ValCoord3D(seed, x2, y4, num3), FastNoise.ValCoord3D(seed, num, y4, num3), FastNoise.ValCoord3D(seed, x3, y4, num3), FastNoise.ValCoord3D(seed, x4, y4, num3), t), t2), FastNoise.CubicLerp(FastNoise.CubicLerp(FastNoise.ValCoord3D(seed, x2, y2, z3), FastNoise.ValCoord3D(seed, num, y2, z3), FastNoise.ValCoord3D(seed, x3, y2, z3), FastNoise.ValCoord3D(seed, x4, y2, z3), t), FastNoise.CubicLerp(FastNoise.ValCoord3D(seed, x2, num2, z3), FastNoise.ValCoord3D(seed, num, num2, z3), FastNoise.ValCoord3D(seed, x3, num2, z3), FastNoise.ValCoord3D(seed, x4, num2, z3), t), FastNoise.CubicLerp(FastNoise.ValCoord3D(seed, x2, y3, z3), FastNoise.ValCoord3D(seed, num, y3, z3), FastNoise.ValCoord3D(seed, x3, y3, z3), FastNoise.ValCoord3D(seed, x4, y3, z3), t), FastNoise.CubicLerp(FastNoise.ValCoord3D(seed, x2, y4, z3), FastNoise.ValCoord3D(seed, num, y4, z3), FastNoise.ValCoord3D(seed, x3, y4, z3), FastNoise.ValCoord3D(seed, x4, y4, z3), t), t2), FastNoise.CubicLerp(FastNoise.CubicLerp(FastNoise.ValCoord3D(seed, x2, y2, z4), FastNoise.ValCoord3D(seed, num, y2, z4), FastNoise.ValCoord3D(seed, x3, y2, z4), FastNoise.ValCoord3D(seed, x4, y2, z4), t), FastNoise.CubicLerp(FastNoise.ValCoord3D(seed, x2, num2, z4), FastNoise.ValCoord3D(seed, num, num2, z4), FastNoise.ValCoord3D(seed, x3, num2, z4), FastNoise.ValCoord3D(seed, x4, num2, z4), t), FastNoise.CubicLerp(FastNoise.ValCoord3D(seed, x2, y3, z4), FastNoise.ValCoord3D(seed, num, y3, z4), FastNoise.ValCoord3D(seed, x3, y3, z4), FastNoise.ValCoord3D(seed, x4, y3, z4), t), FastNoise.CubicLerp(FastNoise.ValCoord3D(seed, x2, y4, z4), FastNoise.ValCoord3D(seed, num, y4, z4), FastNoise.ValCoord3D(seed, x3, y4, z4), FastNoise.ValCoord3D(seed, x4, y4, z4), t), t2), t3) * 0.2962963f;
	}

	public float GetCubicFractal(float x, float y)
	{
		x *= this.m_frequency;
		y *= this.m_frequency;
		switch (this.m_fractalType)
		{
			case FastNoise.FractalType.FBM:
				return this.SingleCubicFractalFBM(x, y);
			case FastNoise.FractalType.Billow:
				return this.SingleCubicFractalBillow(x, y);
			case FastNoise.FractalType.RigidMulti:
				return this.SingleCubicFractalRigidMulti(x, y);
			default:
				return 0f;
		}
	}

	private float SingleCubicFractalFBM(float x, float y)
	{
		int num = this.m_seed;
		float num2 = this.SingleCubic(num, x, y);
		float num3 = 1f;
		int num4 = 0;
		while (++num4 < this.m_octaves)
		{
			x *= this.m_lacunarity;
			y *= this.m_lacunarity;
			num3 *= this.m_gain;
			num2 += this.SingleCubic(++num, x, y) * num3;
		}
		return num2 * this.m_fractalBounding;
	}

	private float SingleCubicFractalBillow(float x, float y)
	{
		int num = this.m_seed;
		float num2 = Math.Abs(this.SingleCubic(num, x, y)) * 2f - 1f;
		float num3 = 1f;
		int num4 = 0;
		while (++num4 < this.m_octaves)
		{
			x *= this.m_lacunarity;
			y *= this.m_lacunarity;
			num3 *= this.m_gain;
			num2 += (Math.Abs(this.SingleCubic(++num, x, y)) * 2f - 1f) * num3;
		}
		return num2 * this.m_fractalBounding;
	}

	private float SingleCubicFractalRigidMulti(float x, float y)
	{
		int num = this.m_seed;
		float num2 = 1f - Math.Abs(this.SingleCubic(num, x, y));
		float num3 = 1f;
		int num4 = 0;
		while (++num4 < this.m_octaves)
		{
			x *= this.m_lacunarity;
			y *= this.m_lacunarity;
			num3 *= this.m_gain;
			num2 -= (1f - Math.Abs(this.SingleCubic(++num, x, y))) * num3;
		}
		return num2;
	}

	public float GetCubic(float x, float y)
	{
		x *= this.m_frequency;
		y *= this.m_frequency;
		return this.SingleCubic(this.m_seed, x, y);
	}

	private float SingleCubic(int seed, float x, float y)
	{
		int num = FastNoise.FastFloor(x);
		int num2 = FastNoise.FastFloor(y);
		int x2 = num - 1;
		int y2 = num2 - 1;
		int x3 = num + 1;
		int y3 = num2 + 1;
		int x4 = num + 2;
		int y4 = num2 + 2;
		float t = x - (float)num;
		float t2 = y - (float)num2;
		return FastNoise.CubicLerp(FastNoise.CubicLerp(FastNoise.ValCoord2D(seed, x2, y2), FastNoise.ValCoord2D(seed, num, y2), FastNoise.ValCoord2D(seed, x3, y2), FastNoise.ValCoord2D(seed, x4, y2), t), FastNoise.CubicLerp(FastNoise.ValCoord2D(seed, x2, num2), FastNoise.ValCoord2D(seed, num, num2), FastNoise.ValCoord2D(seed, x3, num2), FastNoise.ValCoord2D(seed, x4, num2), t), FastNoise.CubicLerp(FastNoise.ValCoord2D(seed, x2, y3), FastNoise.ValCoord2D(seed, num, y3), FastNoise.ValCoord2D(seed, x3, y3), FastNoise.ValCoord2D(seed, x4, y3), t), FastNoise.CubicLerp(FastNoise.ValCoord2D(seed, x2, y4), FastNoise.ValCoord2D(seed, num, y4), FastNoise.ValCoord2D(seed, x3, y4), FastNoise.ValCoord2D(seed, x4, y4), t), t2) * 0.44444445f;
	}

	public float GetCellular(float x, float y, float z)
	{
		x *= this.m_frequency;
		y *= this.m_frequency;
		z *= this.m_frequency;
		FastNoise.CellularReturnType cellularReturnType = this.m_cellularReturnType;
		if (cellularReturnType <= FastNoise.CellularReturnType.Distance)
		{
			return this.SingleCellular(x, y, z);
		}
		return this.SingleCellular2Edge(x, y, z);
	}

	private float SingleCellular(float x, float y, float z)
	{
		int num = FastNoise.FastRound(x);
		int num2 = FastNoise.FastRound(y);
		int num3 = FastNoise.FastRound(z);
		float num4 = 999999f;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		switch (this.m_cellularDistanceFunction)
		{
			case FastNoise.CellularDistanceFunction.Euclidean:
				for (int i = num - 1; i <= num + 1; i++)
				{
					for (int j = num2 - 1; j <= num2 + 1; j++)
					{
						for (int k = num3 - 1; k <= num3 + 1; k++)
						{
							FastNoise.Float3 @float = FastNoise.CELL_3D[FastNoise.Hash3D(this.m_seed, i, j, k) & 255];
							float num8 = (float)i - x + @float.x * this.m_cellularJitter;
							float num9 = (float)j - y + @float.y * this.m_cellularJitter;
							float num10 = (float)k - z + @float.z * this.m_cellularJitter;
							float num11 = num8 * num8 + num9 * num9 + num10 * num10;
							if (num11 < num4)
							{
								num4 = num11;
								num5 = i;
								num6 = j;
								num7 = k;
							}
						}
					}
				}
				break;
			case FastNoise.CellularDistanceFunction.Manhattan:
				for (int l = num - 1; l <= num + 1; l++)
				{
					for (int m = num2 - 1; m <= num2 + 1; m++)
					{
						for (int n = num3 - 1; n <= num3 + 1; n++)
						{
							FastNoise.Float3 float2 = FastNoise.CELL_3D[FastNoise.Hash3D(this.m_seed, l, m, n) & 255];
							float value = (float)l - x + float2.x * this.m_cellularJitter;
							float value2 = (float)m - y + float2.y * this.m_cellularJitter;
							float value3 = (float)n - z + float2.z * this.m_cellularJitter;
							float num12 = Math.Abs(value) + Math.Abs(value2) + Math.Abs(value3);
							if (num12 < num4)
							{
								num4 = num12;
								num5 = l;
								num6 = m;
								num7 = n;
							}
						}
					}
				}
				break;
			case FastNoise.CellularDistanceFunction.Natural:
				for (int num13 = num - 1; num13 <= num + 1; num13++)
				{
					for (int num14 = num2 - 1; num14 <= num2 + 1; num14++)
					{
						for (int num15 = num3 - 1; num15 <= num3 + 1; num15++)
						{
							FastNoise.Float3 float3 = FastNoise.CELL_3D[FastNoise.Hash3D(this.m_seed, num13, num14, num15) & 255];
							float num16 = (float)num13 - x + float3.x * this.m_cellularJitter;
							float num17 = (float)num14 - y + float3.y * this.m_cellularJitter;
							float num18 = (float)num15 - z + float3.z * this.m_cellularJitter;
							float num19 = Math.Abs(num16) + Math.Abs(num17) + Math.Abs(num18) + (num16 * num16 + num17 * num17 + num18 * num18);
							if (num19 < num4)
							{
								num4 = num19;
								num5 = num13;
								num6 = num14;
								num7 = num15;
							}
						}
					}
				}
				break;
		}
		switch (this.m_cellularReturnType)
		{
			case FastNoise.CellularReturnType.CellValue:
				return FastNoise.ValCoord3D(this.m_seed, num5, num6, num7);
			case FastNoise.CellularReturnType.NoiseLookup:
				{
					FastNoise.Float3 float4 = FastNoise.CELL_3D[FastNoise.Hash3D(this.m_seed, num5, num6, num7) & 255];
					return this.m_cellularNoiseLookup.GetNoise((float)num5 + float4.x * this.m_cellularJitter, (float)num6 + float4.y * this.m_cellularJitter, (float)num7 + float4.z * this.m_cellularJitter);
				}
			case FastNoise.CellularReturnType.Distance:
				return num4;
			default:
				return 0f;
		}
	}

	private float SingleCellular2Edge(float x, float y, float z)
	{
		int num = FastNoise.FastRound(x);
		int num2 = FastNoise.FastRound(y);
		int num3 = FastNoise.FastRound(z);
		float[] array = new float[]
		{
			999999f,
			999999f,
			999999f,
			999999f
		};
		switch (this.m_cellularDistanceFunction)
		{
			case FastNoise.CellularDistanceFunction.Euclidean:
				for (int i = num - 1; i <= num + 1; i++)
				{
					for (int j = num2 - 1; j <= num2 + 1; j++)
					{
						for (int k = num3 - 1; k <= num3 + 1; k++)
						{
							FastNoise.Float3 @float = FastNoise.CELL_3D[FastNoise.Hash3D(this.m_seed, i, j, k) & 255];
							float num4 = (float)i - x + @float.x * this.m_cellularJitter;
							float num5 = (float)j - y + @float.y * this.m_cellularJitter;
							float num6 = (float)k - z + @float.z * this.m_cellularJitter;
							float val = num4 * num4 + num5 * num5 + num6 * num6;
							for (int l = this.m_cellularDistanceIndex1; l > 0; l--)
							{
								array[l] = Math.Max(Math.Min(array[l], val), array[l - 1]);
							}
							array[0] = Math.Min(array[0], val);
						}
					}
				}
				break;
			case FastNoise.CellularDistanceFunction.Manhattan:
				for (int m = num - 1; m <= num + 1; m++)
				{
					for (int n = num2 - 1; n <= num2 + 1; n++)
					{
						for (int num7 = num3 - 1; num7 <= num3 + 1; num7++)
						{
							FastNoise.Float3 float2 = FastNoise.CELL_3D[FastNoise.Hash3D(this.m_seed, m, n, num7) & 255];
							float value = (float)m - x + float2.x * this.m_cellularJitter;
							float value2 = (float)n - y + float2.y * this.m_cellularJitter;
							float value3 = (float)num7 - z + float2.z * this.m_cellularJitter;
							float val2 = Math.Abs(value) + Math.Abs(value2) + Math.Abs(value3);
							for (int num8 = this.m_cellularDistanceIndex1; num8 > 0; num8--)
							{
								array[num8] = Math.Max(Math.Min(array[num8], val2), array[num8 - 1]);
							}
							array[0] = Math.Min(array[0], val2);
						}
					}
				}
				break;
			case FastNoise.CellularDistanceFunction.Natural:
				for (int num9 = num - 1; num9 <= num + 1; num9++)
				{
					for (int num10 = num2 - 1; num10 <= num2 + 1; num10++)
					{
						for (int num11 = num3 - 1; num11 <= num3 + 1; num11++)
						{
							FastNoise.Float3 float3 = FastNoise.CELL_3D[FastNoise.Hash3D(this.m_seed, num9, num10, num11) & 255];
							float num12 = (float)num9 - x + float3.x * this.m_cellularJitter;
							float num13 = (float)num10 - y + float3.y * this.m_cellularJitter;
							float num14 = (float)num11 - z + float3.z * this.m_cellularJitter;
							float val3 = Math.Abs(num12) + Math.Abs(num13) + Math.Abs(num14) + (num12 * num12 + num13 * num13 + num14 * num14);
							for (int num15 = this.m_cellularDistanceIndex1; num15 > 0; num15--)
							{
								array[num15] = Math.Max(Math.Min(array[num15], val3), array[num15 - 1]);
							}
							array[0] = Math.Min(array[0], val3);
						}
					}
				}
				break;
		}
		switch (this.m_cellularReturnType)
		{
			case FastNoise.CellularReturnType.Distance2:
				return array[this.m_cellularDistanceIndex1];
			case FastNoise.CellularReturnType.Distance2Add:
				return array[this.m_cellularDistanceIndex1] + array[this.m_cellularDistanceIndex0];
			case FastNoise.CellularReturnType.Distance2Sub:
				return array[this.m_cellularDistanceIndex1] - array[this.m_cellularDistanceIndex0];
			case FastNoise.CellularReturnType.Distance2Mul:
				return array[this.m_cellularDistanceIndex1] * array[this.m_cellularDistanceIndex0];
			case FastNoise.CellularReturnType.Distance2Div:
				return array[this.m_cellularDistanceIndex0] / array[this.m_cellularDistanceIndex1];
			default:
				return 0f;
		}
	}

	public float GetCellular(float x, float y)
	{
		x *= this.m_frequency;
		y *= this.m_frequency;
		FastNoise.CellularReturnType cellularReturnType = this.m_cellularReturnType;
		if (cellularReturnType <= FastNoise.CellularReturnType.Distance)
		{
			return this.SingleCellular(x, y);
		}
		return this.SingleCellular2Edge(x, y);
	}

	private float SingleCellular(float x, float y)
	{
		int num = FastNoise.FastRound(x);
		int num2 = FastNoise.FastRound(y);
		float num3 = 999999f;
		int num4 = 0;
		int num5 = 0;
		switch (this.m_cellularDistanceFunction)
		{
			default:
				for (int i = num - 1; i <= num + 1; i++)
				{
					for (int j = num2 - 1; j <= num2 + 1; j++)
					{
						FastNoise.Float2 @float = FastNoise.CELL_2D[FastNoise.Hash2D(this.m_seed, i, j) & 255];
						float num6 = (float)i - x + @float.x * this.m_cellularJitter;
						float num7 = (float)j - y + @float.y * this.m_cellularJitter;
						float num8 = num6 * num6 + num7 * num7;
						if (num8 < num3)
						{
							num3 = num8;
							num4 = i;
							num5 = j;
						}
					}
				}
				break;
			case FastNoise.CellularDistanceFunction.Manhattan:
				for (int k = num - 1; k <= num + 1; k++)
				{
					for (int l = num2 - 1; l <= num2 + 1; l++)
					{
						FastNoise.Float2 float2 = FastNoise.CELL_2D[FastNoise.Hash2D(this.m_seed, k, l) & 255];
						float value = (float)k - x + float2.x * this.m_cellularJitter;
						float value2 = (float)l - y + float2.y * this.m_cellularJitter;
						float num9 = Math.Abs(value) + Math.Abs(value2);
						if (num9 < num3)
						{
							num3 = num9;
							num4 = k;
							num5 = l;
						}
					}
				}
				break;
			case FastNoise.CellularDistanceFunction.Natural:
				for (int m = num - 1; m <= num + 1; m++)
				{
					for (int n = num2 - 1; n <= num2 + 1; n++)
					{
						FastNoise.Float2 float3 = FastNoise.CELL_2D[FastNoise.Hash2D(this.m_seed, m, n) & 255];
						float num10 = (float)m - x + float3.x * this.m_cellularJitter;
						float num11 = (float)n - y + float3.y * this.m_cellularJitter;
						float num12 = Math.Abs(num10) + Math.Abs(num11) + (num10 * num10 + num11 * num11);
						if (num12 < num3)
						{
							num3 = num12;
							num4 = m;
							num5 = n;
						}
					}
				}
				break;
		}
		switch (this.m_cellularReturnType)
		{
			case FastNoise.CellularReturnType.CellValue:
				return FastNoise.ValCoord2D(this.m_seed, num4, num5);
			case FastNoise.CellularReturnType.NoiseLookup:
				{
					FastNoise.Float2 float4 = FastNoise.CELL_2D[FastNoise.Hash2D(this.m_seed, num4, num5) & 255];
					return this.m_cellularNoiseLookup.GetNoise((float)num4 + float4.x * this.m_cellularJitter, (float)num5 + float4.y * this.m_cellularJitter);
				}
			case FastNoise.CellularReturnType.Distance:
				return num3;
			default:
				return 0f;
		}
	}

	private float SingleCellular2Edge(float x, float y)
	{
		int num = FastNoise.FastRound(x);
		int num2 = FastNoise.FastRound(y);
		float[] array = new float[]
		{
			999999f,
			999999f,
			999999f,
			999999f
		};
		switch (this.m_cellularDistanceFunction)
		{
			default:
				for (int i = num - 1; i <= num + 1; i++)
				{
					for (int j = num2 - 1; j <= num2 + 1; j++)
					{
						FastNoise.Float2 @float = FastNoise.CELL_2D[FastNoise.Hash2D(this.m_seed, i, j) & 255];
						float num3 = (float)i - x + @float.x * this.m_cellularJitter;
						float num4 = (float)j - y + @float.y * this.m_cellularJitter;
						float val = num3 * num3 + num4 * num4;
						for (int k = this.m_cellularDistanceIndex1; k > 0; k--)
						{
							array[k] = Math.Max(Math.Min(array[k], val), array[k - 1]);
						}
						array[0] = Math.Min(array[0], val);
					}
				}
				break;
			case FastNoise.CellularDistanceFunction.Manhattan:
				for (int l = num - 1; l <= num + 1; l++)
				{
					for (int m = num2 - 1; m <= num2 + 1; m++)
					{
						FastNoise.Float2 float2 = FastNoise.CELL_2D[FastNoise.Hash2D(this.m_seed, l, m) & 255];
						float value = (float)l - x + float2.x * this.m_cellularJitter;
						float value2 = (float)m - y + float2.y * this.m_cellularJitter;
						float val2 = Math.Abs(value) + Math.Abs(value2);
						for (int n = this.m_cellularDistanceIndex1; n > 0; n--)
						{
							array[n] = Math.Max(Math.Min(array[n], val2), array[n - 1]);
						}
						array[0] = Math.Min(array[0], val2);
					}
				}
				break;
			case FastNoise.CellularDistanceFunction.Natural:
				for (int num5 = num - 1; num5 <= num + 1; num5++)
				{
					for (int num6 = num2 - 1; num6 <= num2 + 1; num6++)
					{
						FastNoise.Float2 float3 = FastNoise.CELL_2D[FastNoise.Hash2D(this.m_seed, num5, num6) & 255];
						float num7 = (float)num5 - x + float3.x * this.m_cellularJitter;
						float num8 = (float)num6 - y + float3.y * this.m_cellularJitter;
						float val3 = Math.Abs(num7) + Math.Abs(num8) + (num7 * num7 + num8 * num8);
						for (int num9 = this.m_cellularDistanceIndex1; num9 > 0; num9--)
						{
							array[num9] = Math.Max(Math.Min(array[num9], val3), array[num9 - 1]);
						}
						array[0] = Math.Min(array[0], val3);
					}
				}
				break;
		}
		switch (this.m_cellularReturnType)
		{
			case FastNoise.CellularReturnType.Distance2:
				return array[this.m_cellularDistanceIndex1];
			case FastNoise.CellularReturnType.Distance2Add:
				return array[this.m_cellularDistanceIndex1] + array[this.m_cellularDistanceIndex0];
			case FastNoise.CellularReturnType.Distance2Sub:
				return array[this.m_cellularDistanceIndex1] - array[this.m_cellularDistanceIndex0];
			case FastNoise.CellularReturnType.Distance2Mul:
				return array[this.m_cellularDistanceIndex1] * array[this.m_cellularDistanceIndex0];
			case FastNoise.CellularReturnType.Distance2Div:
				return array[this.m_cellularDistanceIndex0] / array[this.m_cellularDistanceIndex1];
			default:
				return 0f;
		}
	}

	public void GradientPerturb(ref float x, ref float y, ref float z)
	{
		this.SingleGradientPerturb(this.m_seed, this.m_gradientPerturbAmp, this.m_frequency, ref x, ref y, ref z);
	}

	public void GradientPerturbFractal(ref float x, ref float y, ref float z)
	{
		int num = this.m_seed;
		float num2 = this.m_gradientPerturbAmp * this.m_fractalBounding;
		float num3 = this.m_frequency;
		this.SingleGradientPerturb(num, num2, this.m_frequency, ref x, ref y, ref z);
		for (int i = 1; i < this.m_octaves; i++)
		{
			num3 *= this.m_lacunarity;
			num2 *= this.m_gain;
			this.SingleGradientPerturb(++num, num2, num3, ref x, ref y, ref z);
		}
	}

	private void SingleGradientPerturb(int seed, float perturbAmp, float frequency, ref float x, ref float y, ref float z)
	{
		float num = x * frequency;
		float num2 = y * frequency;
		float num3 = z * frequency;
		int num4 = FastNoise.FastFloor(num);
		int num5 = FastNoise.FastFloor(num2);
		int num6 = FastNoise.FastFloor(num3);
		int x2 = num4 + 1;
		int y2 = num5 + 1;
		int z2 = num6 + 1;
		float t;
		float t2;
		float t3;
		switch (this.m_interp)
		{
			default:
				t = num - (float)num4;
				t2 = num2 - (float)num5;
				t3 = num3 - (float)num6;
				break;
			case FastNoise.Interp.Hermite:
				t = FastNoise.InterpHermiteFunc(num - (float)num4);
				t2 = FastNoise.InterpHermiteFunc(num2 - (float)num5);
				t3 = FastNoise.InterpHermiteFunc(num3 - (float)num6);
				break;
			case FastNoise.Interp.Quintic:
				t = FastNoise.InterpQuinticFunc(num - (float)num4);
				t2 = FastNoise.InterpQuinticFunc(num2 - (float)num5);
				t3 = FastNoise.InterpQuinticFunc(num3 - (float)num6);
				break;
		}
		FastNoise.Float3 @float = FastNoise.CELL_3D[FastNoise.Hash3D(seed, num4, num5, num6) & 255];
		FastNoise.Float3 float2 = FastNoise.CELL_3D[FastNoise.Hash3D(seed, x2, num5, num6) & 255];
		float a = FastNoise.Lerp(@float.x, float2.x, t);
		float a2 = FastNoise.Lerp(@float.y, float2.y, t);
		float a3 = FastNoise.Lerp(@float.z, float2.z, t);
		FastNoise.Float3 float3 = FastNoise.CELL_3D[FastNoise.Hash3D(seed, num4, y2, num6) & 255];
		float2 = FastNoise.CELL_3D[FastNoise.Hash3D(seed, x2, y2, num6) & 255];
		float b = FastNoise.Lerp(float3.x, float2.x, t);
		float b2 = FastNoise.Lerp(float3.y, float2.y, t);
		float b3 = FastNoise.Lerp(float3.z, float2.z, t);
		float a4 = FastNoise.Lerp(a, b, t2);
		float a5 = FastNoise.Lerp(a2, b2, t2);
		float a6 = FastNoise.Lerp(a3, b3, t2);
		FastNoise.Float3 float4 = FastNoise.CELL_3D[FastNoise.Hash3D(seed, num4, num5, z2) & 255];
		float2 = FastNoise.CELL_3D[FastNoise.Hash3D(seed, x2, num5, z2) & 255];
		a = FastNoise.Lerp(float4.x, float2.x, t);
		a2 = FastNoise.Lerp(float4.y, float2.y, t);
		a3 = FastNoise.Lerp(float4.z, float2.z, t);
		FastNoise.Float3 float5 = FastNoise.CELL_3D[FastNoise.Hash3D(seed, num4, y2, z2) & 255];
		float2 = FastNoise.CELL_3D[FastNoise.Hash3D(seed, x2, y2, z2) & 255];
		b = FastNoise.Lerp(float5.x, float2.x, t);
		b2 = FastNoise.Lerp(float5.y, float2.y, t);
		b3 = FastNoise.Lerp(float5.z, float2.z, t);
		x += FastNoise.Lerp(a4, FastNoise.Lerp(a, b, t2), t3) * perturbAmp;
		y += FastNoise.Lerp(a5, FastNoise.Lerp(a2, b2, t2), t3) * perturbAmp;
		z += FastNoise.Lerp(a6, FastNoise.Lerp(a3, b3, t2), t3) * perturbAmp;
	}

	public void GradientPerturb(ref float x, ref float y)
	{
		this.SingleGradientPerturb(this.m_seed, this.m_gradientPerturbAmp, this.m_frequency, ref x, ref y);
	}

	public void GradientPerturbFractal(ref float x, ref float y)
	{
		int num = this.m_seed;
		float num2 = this.m_gradientPerturbAmp * this.m_fractalBounding;
		float num3 = this.m_frequency;
		this.SingleGradientPerturb(num, num2, this.m_frequency, ref x, ref y);
		for (int i = 1; i < this.m_octaves; i++)
		{
			num3 *= this.m_lacunarity;
			num2 *= this.m_gain;
			this.SingleGradientPerturb(++num, num2, num3, ref x, ref y);
		}
	}

	private void SingleGradientPerturb(int seed, float perturbAmp, float frequency, ref float x, ref float y)
	{
		float num = x * frequency;
		float num2 = y * frequency;
		int num3 = FastNoise.FastFloor(num);
		int num4 = FastNoise.FastFloor(num2);
		int x2 = num3 + 1;
		int y2 = num4 + 1;
		float t;
		float t2;
		switch (this.m_interp)
		{
			default:
				t = num - (float)num3;
				t2 = num2 - (float)num4;
				break;
			case FastNoise.Interp.Hermite:
				t = FastNoise.InterpHermiteFunc(num - (float)num3);
				t2 = FastNoise.InterpHermiteFunc(num2 - (float)num4);
				break;
			case FastNoise.Interp.Quintic:
				t = FastNoise.InterpQuinticFunc(num - (float)num3);
				t2 = FastNoise.InterpQuinticFunc(num2 - (float)num4);
				break;
		}
		FastNoise.Float2 @float = FastNoise.CELL_2D[FastNoise.Hash2D(seed, num3, num4) & 255];
		FastNoise.Float2 float2 = FastNoise.CELL_2D[FastNoise.Hash2D(seed, x2, num4) & 255];
		float a = FastNoise.Lerp(@float.x, float2.x, t);
		float a2 = FastNoise.Lerp(@float.y, float2.y, t);
		FastNoise.Float2 float3 = FastNoise.CELL_2D[FastNoise.Hash2D(seed, num3, y2) & 255];
		float2 = FastNoise.CELL_2D[FastNoise.Hash2D(seed, x2, y2) & 255];
		float b = FastNoise.Lerp(float3.x, float2.x, t);
		float b2 = FastNoise.Lerp(float3.y, float2.y, t);
		x += FastNoise.Lerp(a, b, t2) * perturbAmp;
		y += FastNoise.Lerp(a2, b2, t2) * perturbAmp;
	}

	private const short FN_INLINE = 256;

	private const int FN_CELLULAR_INDEX_MAX = 3;

	private int m_seed = 1337;

	private float m_frequency = 0.01f;

	private FastNoise.Interp m_interp = FastNoise.Interp.Quintic;

	private FastNoise.NoiseType m_noiseType = FastNoise.NoiseType.Simplex;

	private int m_octaves = 3;

	private float m_lacunarity = 2f;

	private float m_gain = 0.5f;

	private FastNoise.FractalType m_fractalType;

	private float m_fractalBounding;

	private FastNoise.CellularDistanceFunction m_cellularDistanceFunction;

	private FastNoise.CellularReturnType m_cellularReturnType;

	private FastNoise m_cellularNoiseLookup;

	private int m_cellularDistanceIndex0;

	private int m_cellularDistanceIndex1 = 1;

	private float m_cellularJitter = 0.45f;

	private float m_gradientPerturbAmp = 1f;

	private static readonly FastNoise.Float2[] GRAD_2D = new FastNoise.Float2[]
	{
		new FastNoise.Float2(-1f, -1f),
		new FastNoise.Float2(1f, -1f),
		new FastNoise.Float2(-1f, 1f),
		new FastNoise.Float2(1f, 1f),
		new FastNoise.Float2(0f, -1f),
		new FastNoise.Float2(-1f, 0f),
		new FastNoise.Float2(0f, 1f),
		new FastNoise.Float2(1f, 0f)
	};

	private static readonly FastNoise.Float3[] GRAD_3D = new FastNoise.Float3[]
	{
		new FastNoise.Float3(1f, 1f, 0f),
		new FastNoise.Float3(-1f, 1f, 0f),
		new FastNoise.Float3(1f, -1f, 0f),
		new FastNoise.Float3(-1f, -1f, 0f),
		new FastNoise.Float3(1f, 0f, 1f),
		new FastNoise.Float3(-1f, 0f, 1f),
		new FastNoise.Float3(1f, 0f, -1f),
		new FastNoise.Float3(-1f, 0f, -1f),
		new FastNoise.Float3(0f, 1f, 1f),
		new FastNoise.Float3(0f, -1f, 1f),
		new FastNoise.Float3(0f, 1f, -1f),
		new FastNoise.Float3(0f, -1f, -1f),
		new FastNoise.Float3(1f, 1f, 0f),
		new FastNoise.Float3(0f, -1f, 1f),
		new FastNoise.Float3(-1f, 1f, 0f),
		new FastNoise.Float3(0f, -1f, -1f)
	};

	private static readonly FastNoise.Float2[] CELL_2D = new FastNoise.Float2[]
	{
		new FastNoise.Float2(-0.2700222f, -0.9628541f),
		new FastNoise.Float2(0.38630927f, -0.9223693f),
		new FastNoise.Float2(0.04444859f, -0.9990117f),
		new FastNoise.Float2(-0.59925234f, -0.80056024f),
		new FastNoise.Float2(-0.781928f, 0.62336874f),
		new FastNoise.Float2(0.9464672f, 0.32279992f),
		new FastNoise.Float2(-0.6514147f, -0.7587219f),
		new FastNoise.Float2(0.93784726f, 0.34704837f),
		new FastNoise.Float2(-0.8497876f, -0.52712524f),
		new FastNoise.Float2(-0.87904257f, 0.47674325f),
		new FastNoise.Float2(-0.8923003f, -0.45144236f),
		new FastNoise.Float2(-0.37984443f, -0.9250504f),
		new FastNoise.Float2(-0.9951651f, 0.09821638f),
		new FastNoise.Float2(0.7724398f, -0.635088f),
		new FastNoise.Float2(0.75732833f, -0.6530343f),
		new FastNoise.Float2(-0.9928005f, -0.119780056f),
		new FastNoise.Float2(-0.05326657f, 0.99858034f),
		new FastNoise.Float2(0.97542536f, -0.22033007f),
		new FastNoise.Float2(-0.76650184f, 0.64224213f),
		new FastNoise.Float2(0.9916367f, 0.12906061f),
		new FastNoise.Float2(-0.99469686f, 0.10285038f),
		new FastNoise.Float2(-0.53792053f, -0.8429955f),
		new FastNoise.Float2(0.50228155f, -0.86470413f),
		new FastNoise.Float2(0.45598215f, -0.8899889f),
		new FastNoise.Float2(-0.8659131f, -0.50019443f),
		new FastNoise.Float2(0.08794584f, -0.9961253f),
		new FastNoise.Float2(-0.5051685f, 0.8630207f),
		new FastNoise.Float2(0.7753185f, -0.6315704f),
		new FastNoise.Float2(-0.69219446f, 0.72171104f),
		new FastNoise.Float2(-0.51916593f, -0.85467345f),
		new FastNoise.Float2(0.8978623f, -0.4402764f),
		new FastNoise.Float2(-0.17067741f, 0.98532695f),
		new FastNoise.Float2(-0.935343f, -0.35374206f),
		new FastNoise.Float2(-0.99924046f, 0.038967468f),
		new FastNoise.Float2(-0.2882064f, -0.9575683f),
		new FastNoise.Float2(-0.96638113f, 0.2571138f),
		new FastNoise.Float2(-0.87597144f, -0.48236302f),
		new FastNoise.Float2(-0.8303123f, -0.55729836f),
		new FastNoise.Float2(0.051101338f, -0.99869347f),
		new FastNoise.Float2(-0.85583735f, -0.51724505f),
		new FastNoise.Float2(0.098870255f, 0.9951003f),
		new FastNoise.Float2(0.9189016f, 0.39448678f),
		new FastNoise.Float2(-0.24393758f, -0.96979094f),
		new FastNoise.Float2(-0.81214094f, -0.5834613f),
		new FastNoise.Float2(-0.99104315f, 0.13354214f),
		new FastNoise.Float2(0.8492424f, -0.52800316f),
		new FastNoise.Float2(-0.9717839f, -0.23587295f),
		new FastNoise.Float2(0.9949457f, 0.10041421f),
		new FastNoise.Float2(0.6241065f, -0.7813392f),
		new FastNoise.Float2(0.6629103f, 0.74869883f),
		new FastNoise.Float2(-0.7197418f, 0.6942418f),
		new FastNoise.Float2(-0.8143371f, -0.58039224f),
		new FastNoise.Float2(0.10452105f, -0.9945227f),
		new FastNoise.Float2(-0.10659261f, -0.99430275f),
		new FastNoise.Float2(0.44579968f, -0.8951328f),
		new FastNoise.Float2(0.105547406f, 0.99441427f),
		new FastNoise.Float2(-0.9927903f, 0.11986445f),
		new FastNoise.Float2(-0.83343667f, 0.55261505f),
		new FastNoise.Float2(0.9115562f, -0.4111756f),
		new FastNoise.Float2(0.8285545f, -0.55990845f),
		new FastNoise.Float2(0.7217098f, -0.6921958f),
		new FastNoise.Float2(0.49404928f, -0.8694339f),
		new FastNoise.Float2(-0.36523214f, -0.9309165f),
		new FastNoise.Float2(-0.9696607f, 0.24445485f),
		new FastNoise.Float2(0.089255095f, -0.9960088f),
		new FastNoise.Float2(0.5354071f, -0.8445941f),
		new FastNoise.Float2(-0.10535762f, 0.9944344f),
		new FastNoise.Float2(-0.98902845f, 0.1477251f),
		new FastNoise.Float2(0.004856105f, 0.9999882f),
		new FastNoise.Float2(0.98855984f, 0.15082914f),
		new FastNoise.Float2(0.92861295f, -0.37104982f),
		new FastNoise.Float2(-0.5832394f, -0.8123003f),
		new FastNoise.Float2(0.30152076f, 0.9534596f),
		new FastNoise.Float2(-0.95751107f, 0.28839657f),
		new FastNoise.Float2(0.9715802f, -0.23671055f),
		new FastNoise.Float2(0.2299818f, 0.97319496f),
		new FastNoise.Float2(0.9557638f, -0.2941352f),
		new FastNoise.Float2(0.7409561f, 0.67155343f),
		new FastNoise.Float2(-0.9971514f, -0.07542631f),
		new FastNoise.Float2(0.69057107f, -0.7232645f),
		new FastNoise.Float2(-0.2907137f, -0.9568101f),
		new FastNoise.Float2(0.5912778f, -0.80646795f),
		new FastNoise.Float2(-0.94545925f, -0.3257405f),
		new FastNoise.Float2(0.66644555f, 0.7455537f),
		new FastNoise.Float2(0.6236135f, 0.78173286f),
		new FastNoise.Float2(0.9126994f, -0.40863165f),
		new FastNoise.Float2(-0.8191762f, 0.57354194f),
		new FastNoise.Float2(-0.8812746f, -0.4726046f),
		new FastNoise.Float2(0.99533135f, 0.09651673f),
		new FastNoise.Float2(0.98556507f, -0.16929697f),
		new FastNoise.Float2(-0.8495981f, 0.52743065f),
		new FastNoise.Float2(0.6174854f, -0.78658235f),
		new FastNoise.Float2(0.85081565f, 0.5254643f),
		new FastNoise.Float2(0.99850327f, -0.0546925f),
		new FastNoise.Float2(0.19713716f, -0.98037595f),
		new FastNoise.Float2(0.66078556f, -0.7505747f),
		new FastNoise.Float2(-0.030974941f, 0.9995202f),
		new FastNoise.Float2(-0.6731661f, 0.73949134f),
		new FastNoise.Float2(-0.71950185f, -0.69449055f),
		new FastNoise.Float2(0.97275114f, 0.2318516f),
		new FastNoise.Float2(0.9997059f, -0.02425069f),
		new FastNoise.Float2(0.44217876f, -0.89692694f),
		new FastNoise.Float2(0.9981351f, -0.061043672f),
		new FastNoise.Float2(-0.9173661f, -0.39804456f),
		new FastNoise.Float2(-0.81500566f, -0.579453f),
		new FastNoise.Float2(-0.87893313f, 0.476945f),
		new FastNoise.Float2(0.015860584f, 0.99987423f),
		new FastNoise.Float2(-0.8095465f, 0.5870558f),
		new FastNoise.Float2(-0.9165899f, -0.39982867f),
		new FastNoise.Float2(-0.8023543f, 0.5968481f),
		new FastNoise.Float2(-0.5176738f, 0.85557806f),
		new FastNoise.Float2(-0.8154407f, -0.57884055f),
		new FastNoise.Float2(0.40220103f, -0.91555136f),
		new FastNoise.Float2(-0.9052557f, -0.4248672f),
		new FastNoise.Float2(0.7317446f, 0.681579f),
		new FastNoise.Float2(-0.56476325f, -0.825253f),
		new FastNoise.Float2(-0.8403276f, -0.54207885f),
		new FastNoise.Float2(-0.93142813f, 0.36392525f),
		new FastNoise.Float2(0.52381986f, 0.85182905f),
		new FastNoise.Float2(0.7432804f, -0.66898f),
		new FastNoise.Float2(-0.9853716f, -0.17041974f),
		new FastNoise.Float2(0.46014687f, 0.88784283f),
		new FastNoise.Float2(0.8258554f, 0.56388193f),
		new FastNoise.Float2(0.6182366f, 0.785992f),
		new FastNoise.Float2(0.83315027f, -0.55304664f),
		new FastNoise.Float2(0.15003075f, 0.9886813f),
		new FastNoise.Float2(-0.6623304f, -0.7492119f),
		new FastNoise.Float2(-0.66859865f, 0.74362344f),
		new FastNoise.Float2(0.7025606f, 0.7116239f),
		new FastNoise.Float2(-0.54193896f, -0.84041786f),
		new FastNoise.Float2(-0.33886164f, 0.9408362f),
		new FastNoise.Float2(0.833153f, 0.55304253f),
		new FastNoise.Float2(-0.29897207f, -0.95426184f),
		new FastNoise.Float2(0.2638523f, 0.9645631f),
		new FastNoise.Float2(0.12410874f, -0.9922686f),
		new FastNoise.Float2(-0.7282649f, -0.6852957f),
		new FastNoise.Float2(0.69625f, 0.71779937f),
		new FastNoise.Float2(-0.91835356f, 0.395761f),
		new FastNoise.Float2(-0.6326102f, -0.7744703f),
		new FastNoise.Float2(-0.9331892f, -0.35938552f),
		new FastNoise.Float2(-0.11537793f, -0.99332166f),
		new FastNoise.Float2(0.9514975f, -0.30765656f),
		new FastNoise.Float2(-0.08987977f, -0.9959526f),
		new FastNoise.Float2(0.6678497f, 0.7442962f),
		new FastNoise.Float2(0.79524004f, -0.6062947f),
		new FastNoise.Float2(-0.6462007f, -0.7631675f),
		new FastNoise.Float2(-0.27335986f, 0.96191186f),
		new FastNoise.Float2(0.966959f, -0.25493184f),
		new FastNoise.Float2(-0.9792895f, 0.20246519f),
		new FastNoise.Float2(-0.5369503f, -0.84361386f),
		new FastNoise.Float2(-0.27003646f, -0.9628501f),
		new FastNoise.Float2(-0.6400277f, 0.76835185f),
		new FastNoise.Float2(-0.78545374f, -0.6189204f),
		new FastNoise.Float2(0.060059056f, -0.9981948f),
		new FastNoise.Float2(-0.024557704f, 0.9996984f),
		new FastNoise.Float2(-0.65983623f, 0.7514095f),
		new FastNoise.Float2(-0.62538946f, -0.7803128f),
		new FastNoise.Float2(-0.6210409f, -0.7837782f),
		new FastNoise.Float2(0.8348889f, 0.55041856f),
		new FastNoise.Float2(-0.15922752f, 0.9872419f),
		new FastNoise.Float2(0.83676225f, 0.54756635f),
		new FastNoise.Float2(-0.8675754f, -0.4973057f),
		new FastNoise.Float2(-0.20226626f, -0.97933054f),
		new FastNoise.Float2(0.939919f, 0.34139755f),
		new FastNoise.Float2(0.98774046f, -0.1561049f),
		new FastNoise.Float2(-0.90344554f, 0.42870283f),
		new FastNoise.Float2(0.12698042f, -0.9919052f),
		new FastNoise.Float2(-0.3819601f, 0.92417884f),
		new FastNoise.Float2(0.9754626f, 0.22016525f),
		new FastNoise.Float2(-0.32040158f, -0.94728184f),
		new FastNoise.Float2(-0.9874761f, 0.15776874f),
		new FastNoise.Float2(0.025353484f, -0.99967855f),
		new FastNoise.Float2(0.4835131f, -0.8753371f),
		new FastNoise.Float2(-0.28508f, -0.9585037f),
		new FastNoise.Float2(-0.06805516f, -0.99768156f),
		new FastNoise.Float2(-0.7885244f, -0.61500347f),
		new FastNoise.Float2(0.3185392f, -0.9479097f),
		new FastNoise.Float2(0.8880043f, 0.45983514f),
		new FastNoise.Float2(0.64769214f, -0.76190215f),
		new FastNoise.Float2(0.98202413f, 0.18875542f),
		new FastNoise.Float2(0.93572754f, -0.35272372f),
		new FastNoise.Float2(-0.88948953f, 0.45695552f),
		new FastNoise.Float2(0.7922791f, 0.6101588f),
		new FastNoise.Float2(0.74838185f, 0.66326815f),
		new FastNoise.Float2(-0.728893f, -0.68462765f),
		new FastNoise.Float2(0.8729033f, -0.48789328f),
		new FastNoise.Float2(0.8288346f, 0.5594937f),
		new FastNoise.Float2(0.08074567f, 0.99673474f),
		new FastNoise.Float2(0.97991484f, -0.1994165f),
		new FastNoise.Float2(-0.5807307f, -0.81409574f),
		new FastNoise.Float2(-0.47000498f, -0.8826638f),
		new FastNoise.Float2(0.2409493f, 0.9705377f),
		new FastNoise.Float2(0.9437817f, -0.33056942f),
		new FastNoise.Float2(-0.89279985f, -0.45045355f),
		new FastNoise.Float2(-0.80696225f, 0.59060305f),
		new FastNoise.Float2(0.062589735f, 0.99803936f),
		new FastNoise.Float2(-0.93125975f, 0.36435598f),
		new FastNoise.Float2(0.57774496f, 0.81621736f),
		new FastNoise.Float2(-0.3360096f, -0.9418586f),
		new FastNoise.Float2(0.69793206f, -0.71616393f),
		new FastNoise.Float2(-0.0020081573f, -0.999998f),
		new FastNoise.Float2(-0.18272944f, -0.98316324f),
		new FastNoise.Float2(-0.6523912f, 0.7578824f),
		new FastNoise.Float2(-0.43026268f, -0.9027037f),
		new FastNoise.Float2(-0.9985126f, -0.054520912f),
		new FastNoise.Float2(-0.010281022f, -0.99994713f),
		new FastNoise.Float2(-0.49460712f, 0.86911666f),
		new FastNoise.Float2(-0.299935f, 0.95395964f),
		new FastNoise.Float2(0.8165472f, 0.5772787f),
		new FastNoise.Float2(0.26974604f, 0.9629315f),
		new FastNoise.Float2(-0.7306287f, -0.68277496f),
		new FastNoise.Float2(-0.7590952f, -0.65097964f),
		new FastNoise.Float2(-0.9070538f, 0.4210146f),
		new FastNoise.Float2(-0.5104861f, -0.859886f),
		new FastNoise.Float2(0.86133504f, 0.5080373f),
		new FastNoise.Float2(0.50078815f, -0.8655699f),
		new FastNoise.Float2(-0.6541582f, 0.7563578f),
		new FastNoise.Float2(-0.83827555f, -0.54524684f),
		new FastNoise.Float2(0.6940071f, 0.7199682f),
		new FastNoise.Float2(0.06950936f, 0.9975813f),
		new FastNoise.Float2(0.17029423f, -0.9853933f),
		new FastNoise.Float2(0.26959732f, 0.9629731f),
		new FastNoise.Float2(0.55196124f, -0.83386976f),
		new FastNoise.Float2(0.2256575f, -0.9742067f),
		new FastNoise.Float2(0.42152628f, -0.9068162f),
		new FastNoise.Float2(0.48818734f, -0.87273884f),
		new FastNoise.Float2(-0.3683855f, -0.92967314f),
		new FastNoise.Float2(-0.98253906f, 0.18605645f),
		new FastNoise.Float2(0.81256473f, 0.582871f),
		new FastNoise.Float2(0.3196461f, -0.947537f),
		new FastNoise.Float2(0.9570914f, 0.28978625f),
		new FastNoise.Float2(-0.6876655f, -0.7260276f),
		new FastNoise.Float2(-0.9988771f, -0.04737673f),
		new FastNoise.Float2(-0.1250179f, 0.9921545f),
		new FastNoise.Float2(-0.82801336f, 0.56070834f),
		new FastNoise.Float2(0.93248636f, -0.36120513f),
		new FastNoise.Float2(0.63946533f, 0.7688199f),
		new FastNoise.Float2(-0.016238471f, -0.99986815f),
		new FastNoise.Float2(-0.99550146f, -0.094746135f),
		new FastNoise.Float2(-0.8145332f, 0.580117f),
		new FastNoise.Float2(0.4037328f, -0.91487694f),
		new FastNoise.Float2(0.9944263f, 0.10543368f),
		new FastNoise.Float2(-0.16247116f, 0.9867133f),
		new FastNoise.Float2(-0.9949488f, -0.10038388f),
		new FastNoise.Float2(-0.69953024f, 0.714603f),
		new FastNoise.Float2(0.5263415f, -0.85027325f),
		new FastNoise.Float2(-0.5395222f, 0.8419714f),
		new FastNoise.Float2(0.65793705f, 0.7530729f),
		new FastNoise.Float2(0.014267588f, -0.9998982f),
		new FastNoise.Float2(-0.6734384f, 0.7392433f),
		new FastNoise.Float2(0.6394121f, -0.7688642f),
		new FastNoise.Float2(0.9211571f, 0.38919085f),
		new FastNoise.Float2(-0.14663722f, -0.98919034f),
		new FastNoise.Float2(-0.7823181f, 0.6228791f),
		new FastNoise.Float2(-0.5039611f, -0.8637264f),
		new FastNoise.Float2(-0.774312f, -0.632804f)
	};

	private static readonly FastNoise.Float3[] CELL_3D = new FastNoise.Float3[]
	{
		new FastNoise.Float3(-0.7292737f, -0.66184396f, 0.17355819f),
		new FastNoise.Float3(0.7902921f, -0.5480887f, -0.2739291f),
		new FastNoise.Float3(0.7217579f, 0.62262124f, -0.3023381f),
		new FastNoise.Float3(0.5656831f, -0.8208298f, -0.079000026f),
		new FastNoise.Float3(0.76004905f, -0.55559796f, -0.33709997f),
		new FastNoise.Float3(0.37139457f, 0.50112647f, 0.78162545f),
		new FastNoise.Float3(-0.12770624f, -0.4254439f, -0.8959289f),
		new FastNoise.Float3(-0.2881561f, -0.5815839f, 0.7607406f),
		new FastNoise.Float3(0.5849561f, -0.6628202f, -0.4674352f),
		new FastNoise.Float3(0.33071712f, 0.039165374f, 0.94291687f),
		new FastNoise.Float3(0.8712122f, -0.41133744f, -0.26793817f),
		new FastNoise.Float3(0.580981f, 0.7021916f, 0.41156778f),
		new FastNoise.Float3(0.5037569f, 0.6330057f, -0.5878204f),
		new FastNoise.Float3(0.44937122f, 0.6013902f, 0.6606023f),
		new FastNoise.Float3(-0.6878404f, 0.090188906f, -0.7202372f),
		new FastNoise.Float3(-0.59589565f, -0.64693505f, 0.47579765f),
		new FastNoise.Float3(-0.5127052f, 0.1946922f, -0.83619875f),
		new FastNoise.Float3(-0.99115074f, -0.054102764f, -0.12121531f),
		new FastNoise.Float3(-0.21497211f, 0.9720882f, -0.09397608f),
		new FastNoise.Float3(-0.7518651f, -0.54280573f, 0.37424695f),
		new FastNoise.Float3(0.5237069f, 0.8516377f, -0.021078179f),
		new FastNoise.Float3(0.6333505f, 0.19261672f, -0.74951047f),
		new FastNoise.Float3(-0.06788242f, 0.39983058f, 0.9140719f),
		new FastNoise.Float3(-0.55386287f, -0.47298968f, -0.6852129f),
		new FastNoise.Float3(-0.72614557f, -0.5911991f, 0.35099334f),
		new FastNoise.Float3(-0.9229275f, -0.17828088f, 0.34120494f),
		new FastNoise.Float3(-0.6968815f, 0.65112746f, 0.30064803f),
		new FastNoise.Float3(0.96080446f, -0.20983632f, -0.18117249f),
		new FastNoise.Float3(0.068171464f, -0.9743405f, 0.21450691f),
		new FastNoise.Float3(-0.3577285f, -0.6697087f, -0.65078455f),
		new FastNoise.Float3(-0.18686211f, 0.7648617f, -0.61649746f),
		new FastNoise.Float3(-0.65416974f, 0.3967915f, 0.64390874f),
		new FastNoise.Float3(0.699334f, -0.6164538f, 0.36182392f),
		new FastNoise.Float3(-0.15466657f, 0.6291284f, 0.7617583f),
		new FastNoise.Float3(-0.6841613f, -0.2580482f, -0.68215424f),
		new FastNoise.Float3(0.5383981f, 0.4258655f, 0.727163f),
		new FastNoise.Float3(-0.5026988f, -0.7939833f, -0.3418837f),
		new FastNoise.Float3(0.32029718f, 0.28344154f, 0.9039196f),
		new FastNoise.Float3(0.86832273f, -0.00037626564f, -0.49599952f),
		new FastNoise.Float3(0.79112005f, -0.085110456f, 0.60571057f),
		new FastNoise.Float3(-0.04011016f, -0.43972486f, 0.8972364f),
		new FastNoise.Float3(0.914512f, 0.35793462f, -0.18854876f),
		new FastNoise.Float3(-0.96120393f, -0.27564842f, 0.010246669f),
		new FastNoise.Float3(0.65103614f, -0.28777993f, -0.70237786f),
		new FastNoise.Float3(-0.20417863f, 0.73652375f, 0.6448596f),
		new FastNoise.Float3(-0.7718264f, 0.37906268f, 0.5104856f),
		new FastNoise.Float3(-0.30600828f, -0.7692988f, 0.56083715f),
		new FastNoise.Float3(0.45400733f, -0.5024843f, 0.73578995f),
		new FastNoise.Float3(0.48167956f, 0.6021208f, -0.636738f),
		new FastNoise.Float3(0.69619805f, -0.32221973f, 0.6414692f),
		new FastNoise.Float3(-0.65321606f, -0.6781149f, 0.33685157f),
		new FastNoise.Float3(0.50893015f, -0.61546624f, -0.60182345f),
		new FastNoise.Float3(-0.16359198f, -0.9133605f, -0.37284088f),
		new FastNoise.Float3(0.5240802f, -0.8437664f, 0.11575059f),
		new FastNoise.Float3(0.5902587f, 0.4983818f, -0.63498837f),
		new FastNoise.Float3(0.5863228f, 0.49476475f, 0.6414308f),
		new FastNoise.Float3(0.6779335f, 0.23413453f, 0.6968409f),
		new FastNoise.Float3(0.7177054f, -0.68589795f, 0.12017863f),
		new FastNoise.Float3(-0.532882f, -0.5205125f, 0.6671608f),
		new FastNoise.Float3(-0.8654874f, -0.07007271f, -0.4960054f),
		new FastNoise.Float3(-0.286181f, 0.79520893f, 0.53454953f),
		new FastNoise.Float3(-0.048495296f, 0.98108363f, -0.18741156f),
		new FastNoise.Float3(-0.63585216f, 0.60583484f, 0.47818002f),
		new FastNoise.Float3(0.62547946f, -0.28616196f, 0.72586966f),
		new FastNoise.Float3(-0.258526f, 0.50619495f, -0.8227582f),
		new FastNoise.Float3(0.021363068f, 0.50640166f, -0.862033f),
		new FastNoise.Float3(0.20011178f, 0.85992634f, 0.46955505f),
		new FastNoise.Float3(0.47435614f, 0.6014985f, -0.6427953f),
		new FastNoise.Float3(0.6622994f, -0.52024746f, -0.539168f),
		new FastNoise.Float3(0.08084973f, -0.65327203f, 0.7527941f),
		new FastNoise.Float3(-0.6893687f, 0.059286036f, 0.7219805f),
		new FastNoise.Float3(-0.11218871f, -0.96731853f, 0.22739525f),
		new FastNoise.Float3(0.7344116f, 0.59796685f, -0.3210533f),
		new FastNoise.Float3(0.5789393f, -0.24888498f, 0.776457f),
		new FastNoise.Float3(0.69881827f, 0.35571697f, -0.6205791f),
		new FastNoise.Float3(-0.86368454f, -0.27487713f, -0.4224826f),
		new FastNoise.Float3(-0.4247028f, -0.46408808f, 0.77733505f),
		new FastNoise.Float3(0.5257723f, -0.84270173f, 0.11583299f),
		new FastNoise.Float3(0.93438303f, 0.31630248f, -0.16395439f),
		new FastNoise.Float3(-0.10168364f, -0.8057303f, -0.58348876f),
		new FastNoise.Float3(-0.6529239f, 0.50602126f, -0.5635893f),
		new FastNoise.Float3(-0.24652861f, -0.9668206f, -0.06694497f),
		new FastNoise.Float3(-0.9776897f, -0.20992506f, -0.0073688254f),
		new FastNoise.Float3(0.7736893f, 0.57342446f, 0.2694238f),
		new FastNoise.Float3(-0.6095088f, 0.4995679f, 0.6155737f),
		new FastNoise.Float3(0.5794535f, 0.7434547f, 0.33392924f),
		new FastNoise.Float3(-0.8226211f, 0.081425816f, 0.56272936f),
		new FastNoise.Float3(-0.51038545f, 0.47036678f, 0.719904f),
		new FastNoise.Float3(-0.5764972f, -0.072316565f, -0.81389266f),
		new FastNoise.Float3(0.7250629f, 0.39499715f, -0.56414634f),
		new FastNoise.Float3(-0.1525424f, 0.48608407f, -0.8604958f),
		new FastNoise.Float3(-0.55509764f, -0.49578208f, 0.6678823f),
		new FastNoise.Float3(-0.18836144f, 0.91458696f, 0.35784173f),
		new FastNoise.Float3(0.76255566f, -0.54144084f, -0.35404897f),
		new FastNoise.Float3(-0.5870232f, -0.3226498f, -0.7424964f),
		new FastNoise.Float3(0.30511242f, 0.2262544f, -0.9250488f),
		new FastNoise.Float3(0.63795763f, 0.57724243f, -0.50970703f),
		new FastNoise.Float3(-0.5966776f, 0.14548524f, -0.7891831f),
		new FastNoise.Float3(-0.65833056f, 0.65554875f, -0.36994147f),
		new FastNoise.Float3(0.74348927f, 0.23510846f, 0.6260573f),
		new FastNoise.Float3(0.5562114f, 0.82643604f, -0.08736329f),
		new FastNoise.Float3(-0.302894f, -0.8251527f, 0.47684193f),
		new FastNoise.Float3(0.11293438f, -0.9858884f, -0.123571075f),
		new FastNoise.Float3(0.5937653f, -0.5896814f, 0.5474657f),
		new FastNoise.Float3(0.6757964f, -0.58357584f, -0.45026484f),
		new FastNoise.Float3(0.7242303f, -0.11527198f, 0.67985505f),
		new FastNoise.Float3(-0.9511914f, 0.0753624f, -0.29925808f),
		new FastNoise.Float3(0.2539471f, -0.18863393f, 0.9486454f),
		new FastNoise.Float3(0.5714336f, -0.16794509f, -0.8032796f),
		new FastNoise.Float3(-0.06778235f, 0.39782694f, 0.9149532f),
		new FastNoise.Float3(0.6074973f, 0.73306f, -0.30589226f),
		new FastNoise.Float3(-0.54354787f, 0.16758224f, 0.8224791f),
		new FastNoise.Float3(-0.5876678f, -0.3380045f, -0.7351187f),
		new FastNoise.Float3(-0.79675627f, 0.040978227f, -0.60290986f),
		new FastNoise.Float3(-0.19963509f, 0.8706295f, 0.4496111f),
		new FastNoise.Float3(-0.027876602f, -0.91062325f, -0.4122962f),
		new FastNoise.Float3(-0.7797626f, -0.6257635f, 0.019757755f),
		new FastNoise.Float3(-0.5211233f, 0.74016446f, -0.42495546f),
		new FastNoise.Float3(0.8575425f, 0.4053273f, -0.31675017f),
		new FastNoise.Float3(0.10452233f, 0.8390196f, -0.53396744f),
		new FastNoise.Float3(0.3501823f, 0.9242524f, -0.15208502f),
		new FastNoise.Float3(0.19878499f, 0.076476134f, 0.9770547f),
		new FastNoise.Float3(0.78459966f, 0.6066257f, -0.12809642f),
		new FastNoise.Float3(0.09006737f, -0.97509897f, -0.20265691f),
		new FastNoise.Float3(-0.82743436f, -0.54229957f, 0.14582036f),
		new FastNoise.Float3(-0.34857976f, -0.41580227f, 0.8400004f),
		new FastNoise.Float3(-0.2471779f, -0.730482f, -0.6366311f),
		new FastNoise.Float3(-0.3700155f, 0.8577948f, 0.35675845f),
		new FastNoise.Float3(0.59133947f, -0.54831195f, -0.59133035f),
		new FastNoise.Float3(0.120487355f, -0.7626472f, -0.6354935f),
		new FastNoise.Float3(0.6169593f, 0.03079648f, 0.7863923f),
		new FastNoise.Float3(0.12581569f, -0.664083f, -0.73699677f),
		new FastNoise.Float3(-0.6477565f, -0.17401473f, -0.74170774f),
		new FastNoise.Float3(0.6217889f, -0.7804431f, -0.06547655f),
		new FastNoise.Float3(0.6589943f, -0.6096988f, 0.44044736f),
		new FastNoise.Float3(-0.26898375f, -0.6732403f, -0.68876356f),
		new FastNoise.Float3(-0.38497752f, 0.56765425f, 0.7277094f),
		new FastNoise.Float3(0.57544446f, 0.81104714f, -0.10519635f),
		new FastNoise.Float3(0.91415936f, 0.3832948f, 0.13190056f),
		new FastNoise.Float3(-0.10792532f, 0.9245494f, 0.36545935f),
		new FastNoise.Float3(0.3779771f, 0.30431488f, 0.87437165f),
		new FastNoise.Float3(-0.21428852f, -0.8259286f, 0.5214617f),
		new FastNoise.Float3(0.58025444f, 0.41480985f, -0.7008834f),
		new FastNoise.Float3(-0.19826609f, 0.85671616f, -0.47615966f),
		new FastNoise.Float3(-0.033815537f, 0.37731808f, -0.9254661f),
		new FastNoise.Float3(-0.68679225f, -0.6656598f, 0.29191336f),
		new FastNoise.Float3(0.7731743f, -0.28757936f, -0.565243f),
		new FastNoise.Float3(-0.09655942f, 0.91937083f, -0.3813575f),
		new FastNoise.Float3(0.27157024f, -0.957791f, -0.09426606f),
		new FastNoise.Float3(0.24510157f, -0.6917999f, -0.6792188f),
		new FastNoise.Float3(0.97770077f, -0.17538553f, 0.115503654f),
		new FastNoise.Float3(-0.522474f, 0.8521607f, 0.029036159f),
		new FastNoise.Float3(-0.77348804f, -0.52612925f, 0.35341796f),
		new FastNoise.Float3(-0.71344924f, -0.26954725f, 0.6467878f),
		new FastNoise.Float3(0.16440372f, 0.5105846f, -0.84396374f),
		new FastNoise.Float3(0.6494636f, 0.055856112f, 0.7583384f),
		new FastNoise.Float3(-0.4711971f, 0.50172806f, -0.7254256f),
		new FastNoise.Float3(-0.63357645f, -0.23816863f, -0.7361091f),
		new FastNoise.Float3(-0.9021533f, -0.2709478f, -0.33571818f),
		new FastNoise.Float3(-0.3793711f, 0.8722581f, 0.3086152f),
		new FastNoise.Float3(-0.68555987f, -0.32501432f, 0.6514394f),
		new FastNoise.Float3(0.29009423f, -0.7799058f, -0.5546101f),
		new FastNoise.Float3(-0.20983194f, 0.8503707f, 0.48253515f),
		new FastNoise.Float3(-0.45926037f, 0.6598504f, -0.5947077f),
		new FastNoise.Float3(0.87159455f, 0.09616365f, -0.48070312f),
		new FastNoise.Float3(-0.6776666f, 0.71185046f, -0.1844907f),
		new FastNoise.Float3(0.7044378f, 0.3124276f, 0.637304f),
		new FastNoise.Float3(-0.7052319f, -0.24010932f, -0.6670798f),
		new FastNoise.Float3(0.081921004f, -0.72073364f, -0.68835455f),
		new FastNoise.Float3(-0.6993681f, -0.5875763f, -0.4069869f),
		new FastNoise.Float3(-0.12814544f, 0.6419896f, 0.75592864f),
		new FastNoise.Float3(-0.6337388f, -0.67854714f, -0.3714147f),
		new FastNoise.Float3(0.5565052f, -0.21688876f, -0.8020357f),
		new FastNoise.Float3(-0.57915545f, 0.7244372f, -0.3738579f),
		new FastNoise.Float3(0.11757791f, -0.7096451f, 0.69467926f),
		new FastNoise.Float3(-0.613462f, 0.13236311f, 0.7785528f),
		new FastNoise.Float3(0.69846356f, -0.029805163f, -0.7150247f),
		new FastNoise.Float3(0.83180827f, -0.3930172f, 0.39195976f),
		new FastNoise.Float3(0.14695764f, 0.055416517f, -0.98758924f),
		new FastNoise.Float3(0.70886856f, -0.2690504f, 0.65201014f),
		new FastNoise.Float3(0.27260533f, 0.67369765f, -0.68688995f),
		new FastNoise.Float3(-0.65912956f, 0.30354586f, -0.68804663f),
		new FastNoise.Float3(0.48151314f, -0.752827f, 0.4487723f),
		new FastNoise.Float3(0.943001f, 0.16756473f, -0.28752613f),
		new FastNoise.Float3(0.43480295f, 0.7695305f, -0.46772778f),
		new FastNoise.Float3(0.39319962f, 0.5944736f, 0.70142365f),
		new FastNoise.Float3(0.72543365f, -0.60392565f, 0.33018148f),
		new FastNoise.Float3(0.75902355f, -0.6506083f, 0.024333132f),
		new FastNoise.Float3(-0.8552769f, -0.3430043f, 0.38839358f),
		new FastNoise.Float3(-0.6139747f, 0.6981725f, 0.36822575f),
		new FastNoise.Float3(-0.74659055f, -0.575201f, 0.33428493f),
		new FastNoise.Float3(0.5730066f, 0.8105555f, -0.12109168f),
		new FastNoise.Float3(-0.92258775f, -0.3475211f, -0.16751404f),
		new FastNoise.Float3(-0.71058166f, -0.47196922f, -0.5218417f),
		new FastNoise.Float3(-0.0856461f, 0.35830015f, 0.9296697f),
		new FastNoise.Float3(-0.8279698f, -0.2043157f, 0.5222271f),
		new FastNoise.Float3(0.42794403f, 0.278166f, 0.8599346f),
		new FastNoise.Float3(0.539908f, -0.78571206f, -0.3019204f),
		new FastNoise.Float3(0.5678404f, -0.5495414f, -0.61283076f),
		new FastNoise.Float3(-0.9896071f, 0.13656391f, -0.045034185f),
		new FastNoise.Float3(-0.6154343f, -0.64408755f, 0.45430374f),
		new FastNoise.Float3(0.10742044f, -0.79463404f, 0.59750944f),
		new FastNoise.Float3(-0.359545f, -0.888553f, 0.28495783f),
		new FastNoise.Float3(-0.21804053f, 0.1529889f, 0.9638738f),
		new FastNoise.Float3(-0.7277432f, -0.61640507f, -0.30072346f),
		new FastNoise.Float3(0.7249729f, -0.0066971947f, 0.68874484f),
		new FastNoise.Float3(-0.5553659f, -0.5336586f, 0.6377908f),
		new FastNoise.Float3(0.5137558f, 0.79762083f, -0.316f),
		new FastNoise.Float3(-0.3794025f, 0.92456084f, -0.035227515f),
		new FastNoise.Float3(0.82292485f, 0.27453658f, -0.49741766f),
		new FastNoise.Float3(-0.5404114f, 0.60911417f, 0.5804614f),
		new FastNoise.Float3(0.8036582f, -0.27030295f, 0.5301602f),
		new FastNoise.Float3(0.60443187f, 0.68329686f, 0.40959433f),
		new FastNoise.Float3(0.06389989f, 0.96582085f, -0.2512108f),
		new FastNoise.Float3(0.10871133f, 0.74024713f, -0.6634878f),
		new FastNoise.Float3(-0.7134277f, -0.6926784f, 0.10591285f),
		new FastNoise.Float3(0.64588976f, -0.57245487f, -0.50509584f),
		new FastNoise.Float3(-0.6553931f, 0.73814714f, 0.15999562f),
		new FastNoise.Float3(0.39109614f, 0.91888714f, -0.05186756f),
		new FastNoise.Float3(-0.48790225f, -0.5904377f, 0.64291114f),
		new FastNoise.Float3(0.601479f, 0.77074414f, -0.21018201f),
		new FastNoise.Float3(-0.5677173f, 0.7511361f, 0.33688518f),
		new FastNoise.Float3(0.7858574f, 0.22667466f, 0.5753667f),
		new FastNoise.Float3(-0.45203456f, -0.6042227f, -0.65618575f),
		new FastNoise.Float3(0.0022721163f, 0.4132844f, -0.9105992f),
		new FastNoise.Float3(-0.58157516f, -0.5162926f, 0.6286591f),
		new FastNoise.Float3(-0.03703705f, 0.8273786f, 0.5604221f),
		new FastNoise.Float3(-0.51196927f, 0.79535437f, -0.324498f),
		new FastNoise.Float3(-0.26824173f, -0.957229f, -0.10843876f),
		new FastNoise.Float3(-0.23224828f, -0.9679131f, -0.09594243f),
		new FastNoise.Float3(0.3554329f, -0.8881506f, 0.29130062f),
		new FastNoise.Float3(0.73465204f, -0.4371373f, 0.5188423f),
		new FastNoise.Float3(0.998512f, 0.046590112f, -0.028339446f),
		new FastNoise.Float3(-0.37276876f, -0.9082481f, 0.19007573f),
		new FastNoise.Float3(0.9173738f, -0.3483642f, 0.19252984f),
		new FastNoise.Float3(0.2714911f, 0.41475296f, -0.86848867f),
		new FastNoise.Float3(0.5131763f, -0.71163344f, 0.4798207f),
		new FastNoise.Float3(-0.87373537f, 0.18886992f, -0.44823506f),
		new FastNoise.Float3(0.84600437f, -0.3725218f, 0.38145f),
		new FastNoise.Float3(0.89787275f, -0.17802091f, -0.40265754f),
		new FastNoise.Float3(0.21780656f, -0.9698323f, -0.10947895f),
		new FastNoise.Float3(-0.15180314f, -0.7788918f, -0.6085091f),
		new FastNoise.Float3(-0.2600385f, -0.4755398f, -0.840382f),
		new FastNoise.Float3(0.5723135f, -0.7474341f, -0.33734185f),
		new FastNoise.Float3(-0.7174141f, 0.16990171f, -0.67561114f),
		new FastNoise.Float3(-0.6841808f, 0.021457076f, -0.72899675f),
		new FastNoise.Float3(-0.2007448f, 0.06555606f, -0.9774477f),
		new FastNoise.Float3(-0.11488037f, -0.8044887f, 0.5827524f),
		new FastNoise.Float3(-0.787035f, 0.03447489f, 0.6159443f),
		new FastNoise.Float3(-0.20155965f, 0.68598723f, 0.69913894f),
		new FastNoise.Float3(-0.085810825f, -0.10920836f, -0.99030805f),
		new FastNoise.Float3(0.5532693f, 0.73252505f, -0.39661077f),
		new FastNoise.Float3(-0.18424894f, -0.9777375f, -0.100407675f),
		new FastNoise.Float3(0.07754738f, -0.9111506f, 0.40471104f),
		new FastNoise.Float3(0.13998385f, 0.7601631f, -0.63447344f),
		new FastNoise.Float3(0.44844192f, -0.84528923f, 0.29049253f)
	};

	private const int X_PRIME = 1619;

	private const int Y_PRIME = 31337;

	private const int Z_PRIME = 6971;

	private const int W_PRIME = 1013;

	private const float F3 = 0.33333334f;

	private const float G3 = 0.16666667f;

	private const float G33 = -0.5f;

	private const float SQRT3 = 1.7320508f;

	private const float F2 = 0.3660254f;

	private const float G2 = 0.21132487f;

	private static readonly byte[] SIMPLEX_4D = new byte[]
	{
		0,
		1,
		2,
		3,
		0,
		1,
		3,
		2,
		0,
		0,
		0,
		0,
		0,
		2,
		3,
		1,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		1,
		2,
		3,
		0,
		0,
		2,
		1,
		3,
		0,
		0,
		0,
		0,
		0,
		3,
		1,
		2,
		0,
		3,
		2,
		1,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		1,
		3,
		2,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		1,
		2,
		0,
		3,
		0,
		0,
		0,
		0,
		1,
		3,
		0,
		2,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		2,
		3,
		0,
		1,
		2,
		3,
		1,
		0,
		1,
		0,
		2,
		3,
		1,
		0,
		3,
		2,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		2,
		0,
		3,
		1,
		0,
		0,
		0,
		0,
		2,
		1,
		3,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		2,
		0,
		1,
		3,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		3,
		0,
		1,
		2,
		3,
		0,
		2,
		1,
		0,
		0,
		0,
		0,
		3,
		1,
		2,
		0,
		2,
		1,
		0,
		3,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		3,
		1,
		0,
		2,
		0,
		0,
		0,
		0,
		3,
		2,
		0,
		1,
		3,
		2,
		1,
		0
	};

	private const float F4 = 0.309017f;

	private const float G4 = 0.1381966f;

	private const float CUBIC_3D_BOUNDING = 0.2962963f;

	private const float CUBIC_2D_BOUNDING = 0.44444445f;

	public enum NoiseType
	{

		Value,

		ValueFractal,

		Perlin,

		PerlinFractal,

		Simplex,

		SimplexFractal,

		Cellular,

		WhiteNoise,

		Cubic,

		CubicFractal
	}

	public enum Interp
	{

		Linear,

		Hermite,

		Quintic
	}


	public enum FractalType
	{

		FBM,

		Billow,

		RigidMulti
	}


	public enum CellularDistanceFunction
	{

		Euclidean,

		Manhattan,

		Natural
	}


	public enum CellularReturnType
	{

		CellValue,

		NoiseLookup,

		Distance,

		Distance2,

		Distance2Add,

		Distance2Sub,

		Distance2Mul,

		Distance2Div
	}

	private struct Float2
	{

		public Float2(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public readonly float x;

		public readonly float y;
	}

	private struct Float3
	{

		public Float3(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public readonly float x;

		public readonly float y;

		public readonly float z;
	}
}