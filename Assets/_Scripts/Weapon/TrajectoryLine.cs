using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRen;
    [SerializeField] private int count;

    private WeaponShooting shooter;
    private Transform firePoint;
    private float WEAPONRANGE;

    private const float ratio = 1f;
    private const float GRAVITY = 9.81f;
    public void SetShooter(WeaponController target)
    {
        shooter = target.shooter;
        firePoint = shooter.firePoint;
        WEAPONRANGE = target.weapon.range;
    }

    private void Update()
    {
        UpdateTrajectory();
    }
    private void UpdateTrajectory()
    {
        lineRen.transform.position = firePoint.position; // cannon set as firepoint child because of rotation
        lineRen.positionCount = (int)(count * ratio + 1);
        float totalLength = 0;
        float power = shooter.Power;
        Vector2 direction = shooter.Direction;
        float angle = shooter.Angle;
        Vector2 force = Mathf.Sqrt(WEAPONRANGE * 10) * power * direction;

        float flightTime = (float)Mathf.Abs(2 * force.y / GRAVITY);
        float range = (float)Mathf.Abs(force.x) * flightTime * 2;

        float lenghtLimit = (float)power * WEAPONRANGE * 0.7f;
        //
        float AbsAngle = Mathf.Abs(angle);
        if ( AbsAngle.InRange(0,45) || AbsAngle.InRange(135, 180))
        {
            range = (float)lenghtLimit * 2;
        }
        //


        for (int i = 0; i <= count * ratio; i++) {
            int multiplier = force.x > 0 ? 1 : -1;
            float X = (float)range / count * i * multiplier;
            float t = (float)Mathf.Abs((float)X / force.x);
            float Y = force.y*t - GRAVITY*(float)Mathf.Pow(t, 2) / 2;
            Vector2 position = new(X, Y);
            if (i != 0)
            {
                float lengthToPrevPoint = (position - (Vector2)lineRen.GetPosition(i - 1)).magnitude;
                totalLength += lengthToPrevPoint;
            }
            if (totalLength > lenghtLimit) //position.magnitude > limit
            {
                lineRen.positionCount = i;
                return;
            }
            lineRen.SetPosition(i, position);
        }
    }
}
