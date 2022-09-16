using UnityEngine;

public static class MathLib
{

	public static Vector3 FirstOrderIntercept(Vector3 shooterPosition, Vector3 shooterVelocity, float projectileVelocity, Vector3 targetPosition, Vector3 targetVelocity)
	{
		Vector3 targetRelativePosition = targetPosition - shooterPosition;
		Vector3 vector = targetVelocity - shooterVelocity;
		float d = MathLib.FirstOrderInterceptTime(projectileVelocity, targetRelativePosition, vector);
		return targetPosition + d * vector;
	}

	private static float FirstOrderInterceptTime(float projectileVelocity, Vector3 targetRelativePosition, Vector3 targetRelativeVelocity)
	{
		float sqrMagnitude = targetRelativeVelocity.sqrMagnitude;
		if (sqrMagnitude < 0.001f)
		{
			return 0f;
		}
		float num = sqrMagnitude - projectileVelocity * projectileVelocity;
		if (Mathf.Abs(num) < 0.001f)
		{
			return Mathf.Max(-targetRelativePosition.sqrMagnitude / (2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition)), 0f);
		}
		float num2 = 2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
		float sqrMagnitude2 = targetRelativePosition.sqrMagnitude;
		float num3 = num2 * num2 - 4f * num * sqrMagnitude2;
		if (num3 > 0f)
		{
			float num4 = (-num2 + Mathf.Sqrt(num3)) / (2f * num);
			float num5 = (-num2 - Mathf.Sqrt(num3)) / (2f * num);
			if (num4 <= 0f)
			{
				return Mathf.Max(num5, 0f);
			}
			if (num5 > 0f)
			{
				return Mathf.Min(num4, num5);
			}
			return num4;
		}
		else
		{
			if (num3 < 0f)
			{
				return 0f;
			}
			return Mathf.Max(-num2 / (2f * num), 0f);
		}
	}

	public static float Map(float v, float fromMin, float fromMax, float toMin, float toMax)
	{
		v = Mathf.Clamp(v, fromMin, fromMax);
		float num = v - fromMin;
		float num2 = fromMax - fromMin;
		float num3 = num / num2;
		return (toMax - toMin) * num3 + toMin;
	}

	public static float NormalizeAngle(float angle)
	{
		angle %= 360f;
		if (angle < 0f)
		{
			angle += 360f;
		}
		return angle;
	}

	public static float SignedAngle(float angle)
	{
		angle = MathLib.NormalizeAngle(angle);
		if (angle > 180f)
		{
			angle = -(360f - angle);
		}
		return angle;
	}

	public static float ClampAngle(float angle, float from, float to)
	{
		angle = MathLib.NormalizeAngle(angle);
		if (angle > 180f)
		{
			angle = -(360f - angle);
		}
		angle = Mathf.Clamp(angle, from, to);
		if (angle < 0f)
		{
			angle = 360f + angle;
		}
		return angle;
	}

	public static bool Compare(float a, float b, float epsilon = 0.0001f)
	{
		return Mathf.Abs(a - b) <= epsilon;
	}

	public static bool Compare(Vector3 a, Vector3 b, float epsilon = 0.0001f)
	{
		return Mathf.Abs(a.x - b.x) <= epsilon && Mathf.Abs(a.y - b.y) <= epsilon && Mathf.Abs(a.z - b.z) <= epsilon;
	}

	public static bool CompareSign(float a, float b)
	{
		return Mathf.Approximately(Mathf.Sign(a), Mathf.Sign(b));
	}

	public static float LerpAngleUnclamped(float a, float b, float t)
	{
		float num = Mathf.Repeat(b - a, 360f);
		if (num > 180f)
		{
			num -= 360f;
		}
		return a + num * t;
	}

	public static Quaternion Damp(Quaternion a, Quaternion b, float smoothing, float deltaTime)
	{
		return Quaternion.Slerp(a, b, 1f - Mathf.Exp(-smoothing * deltaTime));
	}

	public static Vector3 DampDirection(Vector3 a, Vector3 b, float smoothing, float deltaTime)
	{
		return Vector3.Slerp(a, b, 1f - Mathf.Exp(-smoothing * deltaTime));
	}

	public static Vector3 Damp(Vector3 a, Vector3 b, float smoothing, float deltaTime)
	{
		return Vector3.Lerp(a, b, 1f - Mathf.Exp(-smoothing * deltaTime));
	}

	public static Color Damp(Color a, Color b, float smoothing, float deltaTime)
	{
		return Color.Lerp(a, b, 1f - Mathf.Exp(-smoothing * deltaTime));
	}

	public static float Damp(float a, float b, float smoothing, float deltaTime)
	{
		return Mathf.Lerp(a, b, 1f - Mathf.Exp(-smoothing * deltaTime));
	}

	public static float DampAngle(float a, float b, float smoothing, float deltaTime)
	{
		return Mathf.LerpAngle(a, b, 1f - Mathf.Exp(-smoothing * deltaTime));
	}

	public static float InverseLerp(Vector3 a, Vector3 b, Vector3 t)
	{
		Vector3 vector = b - a;
		return Mathf.Clamp01(Vector3.Dot(t - a, vector) / Vector3.Dot(vector, vector));
	}

	public static float InverseLerpUnclamped(Vector3 a, Vector3 b, Vector3 t)
	{
		Vector3 vector = b - a;
		return Vector3.Dot(t - a, vector) / Vector3.Dot(vector, vector);
	}

	public static Vector3 RandomOnTriangle(Vector3 a, Vector3 b, Vector3 c)
	{
		float value = UnityEngine.Random.value;
		float value2 = UnityEngine.Random.value;
		return (1f - Mathf.Sqrt(value)) * a + Mathf.Sqrt(value) * (1f - value2) * b + value2 * Mathf.Sqrt(value) * c;
	}

	public static bool PlanePlaneIntersection(out Vector3 linePoint, out Vector3 lineVec, Vector3 plane1Normal, Vector3 plane1Position, Vector3 plane2Normal, Vector3 plane2Position)
	{
		linePoint = Vector3.zero;
		lineVec = Vector3.zero;
		lineVec = Vector3.Cross(plane1Normal, plane2Normal);
		Vector3 vector = Vector3.Cross(plane2Normal, lineVec);
		float num = Vector3.Dot(plane1Normal, vector);
		if (Mathf.Abs(num) > 0.006f)
		{
			Vector3 rhs = plane1Position - plane2Position;
			float d = Vector3.Dot(plane1Normal, rhs) / num;
			linePoint = plane2Position + d * vector;
			return true;
		}
		return false;
	}

	public static bool IntersectRaySphere(Vector3 start, Vector3 direction, Vector3 center, float radius, out Vector3 point)
	{
		point = Vector3.zero;
		Vector3 vector = start - center;
		float num = Vector3.Dot(vector, direction);
		float num2 = Vector3.Dot(vector, vector) - radius * radius;
		if (num2 > 0f && num > 0f)
		{
			return false;
		}
		float num3 = num * num - num2;
		if (num3 < 0f)
		{
			return false;
		}
		float num4 = -num - Mathf.Sqrt(num3);
		num4 = Mathf.Max(num4, 0f);
		point = start + num4 * direction;
		return true;
	}

	public static Vector3 ClosestPointOnLineSegment(Vector3 start, Vector3 end, Vector3 position)
	{
		Vector3 lhs = position - start;
		Vector3 vector = end - start;
		float num = Vector3.Dot(lhs, vector) / vector.sqrMagnitude;
		num = Mathf.Clamp01(num);
		return start + num * vector;
	}

	public static Vector3 ProjectPointOnPlane(Vector3 planeNormal, Vector3 planePoint, Vector3 point)
	{
		float num = MathLib.SignedDistancePlanePoint(planeNormal, planePoint, point);
		num *= -1f;
		Vector3 b = planeNormal.normalized * num;
		return point + b;
	}

	public static float SignedDistancePlanePoint(Vector3 planeNormal, Vector3 planePoint, Vector3 point)
	{
		return Vector3.Dot(planeNormal, point - planePoint);
	}

	public static Vector3 CalculateCubicBezierPoint(Vector3 start, Vector3 end, Vector3 startHandle, Vector3 endHandle, float t)
	{
		float num = 1f - t;
		float num2 = t * t;
		float num3 = num * num;
		float d = num3 * num;
		float d2 = num2 * t;
		return d * start + 3f * num3 * t * startHandle + 3f * num * num2 * endHandle + d2 * end;
	}

	public static float SmoothMin(float a, float b, float k)
	{
		k = Mathf.Max(0f, k);
		float num = Mathf.Max(0f, Mathf.Min(1f, (b - a + k) / (2f * k)));
		return a * num + b * (1f - num) - k * num * (1f - num);
	}

	public static float SmoothMax(float a, float b, float k)
	{
		k = Mathf.Min(0f, -k);
		float num = Mathf.Max(0f, Mathf.Min(1f, (b - a + k) / (2f * k)));
		return a * num + b * (1f - num) - k * num * (1f - num);
	}

	public static bool DistanceCheck(Vector2 a, Vector2 b, float distance)
	{
		return (a - b).sqrMagnitude <= distance * distance;
	}

	public static Vector3 CalculateNormal(Vector3 a, Vector3 b, Vector3 c)
	{
		Vector3 vector = b - a;
		Vector3 vector2 = c - a;
		return new Vector3(vector.y * vector2.z - vector.z * vector2.y, vector.z * vector2.x - vector.x * vector2.z, vector.x * vector2.y - vector.y * vector2.x).normalized;
	}

	public static float ScreenSpaceDistance(Camera camera, Vector3 a, Vector3 b)
	{
		if (!camera)
		{
			return float.PositiveInfinity;
		}
		Vector3 b2 = new Vector3(1f / (float)camera.pixelWidth, 1f / (float)camera.pixelHeight, 0f);
		a = Vector3.Scale(camera.WorldToScreenPoint(a), b2);
		b = Vector3.Scale(camera.WorldToScreenPoint(b), b2);
		return Vector3.Distance(a, b);
	}

	public static Vector3 ProjectPointOnLine(Vector3 linePoint, Vector3 lineVec, Vector3 point)
	{
		float d = Vector3.Dot(point - linePoint, lineVec);
		return linePoint + lineVec * d;
	}

	public static Vector3 ProjectPointOnLineSegment(Vector3 linePoint1, Vector3 linePoint2, Vector3 point)
	{
		Vector3 vector = MathLib.ProjectPointOnLine(linePoint1, (linePoint2 - linePoint1).normalized, point);
		int num = MathLib.PointOnWhichSideOfLineSegment(linePoint1, linePoint2, vector);
		if (num == 0)
		{
			return vector;
		}
		if (num == 1)
		{
			return linePoint1;
		}
		if (num == 2)
		{
			return linePoint2;
		}
		return Vector3.zero;
	}

	public static int PointOnWhichSideOfLineSegment(Vector3 linePoint1, Vector3 linePoint2, Vector3 point)
	{
		Vector3 rhs = linePoint2 - linePoint1;
		Vector3 lhs = point - linePoint1;
		if (Vector3.Dot(lhs, rhs) <= 0f)
		{
			return 1;
		}
		if (lhs.magnitude <= rhs.magnitude)
		{
			return 0;
		}
		return 2;
	}

	public static Vector3 GetRandomDirectionInCone(Vector3 axis, float maxAngle)
	{
		float d = Mathf.Tan(0.017453292f * maxAngle / 2f);
		Vector2 vector = UnityEngine.Random.insideUnitCircle * d;
		return (axis + Quaternion.FromToRotation(Vector3.forward, axis) * new Vector3(vector.x, vector.y, 0f)).normalized;
	}

	public static Vector2 RotateClockwise(Vector2 direction, float angle)
	{
		angle *= 0.017453292f;
		float num = Mathf.Sin(angle);
		float num2 = Mathf.Cos(angle);
		return new Vector2(direction.x * num2 - direction.y * num, direction.y * num2 + direction.x * num);
	}

	public static Vector3 GetRandomDirection2D(MathLib.Axis axis = MathLib.Axis.Y)
	{
		Vector2 normalized = UnityEngine.Random.insideUnitCircle.normalized;
		if (axis == MathLib.Axis.X)
		{
			return new Vector3(0f, normalized.x, normalized.y);
		}
		if (axis == MathLib.Axis.Y)
		{
			return new Vector3(normalized.x, 0f, normalized.y);
		}
		return new Vector3(normalized.x, normalized.y, 0f);
	}

	public static Vector3 GetRandomInsideCircle2D(MathLib.Axis axis = MathLib.Axis.Y)
	{
		Vector2 insideUnitCircle = Random.insideUnitCircle;
		if (axis == MathLib.Axis.X)
		{
			return new Vector3(0f, insideUnitCircle.x, insideUnitCircle.y);
		}
		if (axis == MathLib.Axis.Y)
		{
			return new Vector3(insideUnitCircle.x, 0f, insideUnitCircle.y);
		}
		return new Vector3(insideUnitCircle.x, insideUnitCircle.y, 0f);
	}

	public static bool CompareVelocities(Vector3 a, Vector3 b, float maxAngle, float maxSpeed)
	{
		float magnitude = a.magnitude;
		float magnitude2 = b.magnitude;
		if (Mathf.Abs(magnitude - magnitude2) > maxSpeed)
		{
			return false;
		}
		a /= magnitude;
		b /= magnitude2;
		return Vector3.Angle(a, b) <= maxAngle;
	}

	public const float Tau = 6.2831855f;

	public const double DegToRad = 0.017453292519943295;

	public const double RadToDeg = 57.29577951308232;

	public enum Axis
	{
		X,

		Y,

		Z
	}
}
