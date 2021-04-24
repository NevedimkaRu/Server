using RAGE;
using System;
using System.Collections.Generic;
using System.Text;

namespace cs_packages.model
{
    public class VehicleHandling
    {
        public int CarId { get; set; }
        public int Slot { get; set; }
        //Физические параметры
        public float fInitialDragCoeff { get; set; } //Начальный коэффициент аэродинамического сопротивления (от 0 до 120);
        public Vector3 vecCentreOfMassOffset { get; set; }
        /*координаты x,y,z, отвечающие за вектор центра массы (от -10.0 до 10.0);
        x - положительное значение смещает центр тяжести вправо.
        y - положительное значение смещает центр тяжести вперед.
        z - положительное значение смещает центр тяжести вверх.*/
        public Vector3 vecInertiaMultiplier { get; set; } //Координаты x,y,z, отвечающие за множитель вектора инерции (от -10. до 10.0)

        //Параметры трансмиссии
        public float fDriveBiasFront { get; set; } //параметр, отвечающий за привод транспортного средства. Так, значение 0.0 определяет заднеприводный транспорт; 1.0 – переднеприводный; 0.5 – полноприводный транспорт (на 4 колеса).
        public int nInitialDriveGears { get; set; } //Параметр, отвечающий за количество передач (от 1 до 16);
        public float fInitialDriveForce { get; set; } //Множитель тягового усиления (ускорение) транспортного средства (от 0.01 до 2.0. Значение 1.0 не влияет на множитель.);
        public float fDriveInertia { get; set; } //Множитель, отвечающий за инерцию двигателя (за то, как быстро набираются обороты, от 0.01 до 2.0. Значение 1.0 не влияет на множитель.);
        public float fClutchChangeRateScaleUpShift { get; set; } //Множитель сцепления при смещении вверх;
        public float fClutchChangeRateScaleDownShift { get; set; } //Множитель сцепления при смещении вниз
        public float fInitialDriveMaxFlatVel { get; set; } //Теоретическая максимальная скорость транспортного средства в км/ч (от 0.00 до 500.0 и более. Изменение данного значения не гарантирует достижение этой скорости);
        public float fBrakeForce { get; set; } //Множитель силы торможения транспортного средства (от 0.01 до 2.0. Значение 1.0 не влияет на множитель);
        public float fBrakeBiasFront { get; set; } //Параметр, отвечающий за равномерное распределение силы торможения между передней и задней осей (от 0.0 до 1.0. Значение 0.0 увеличивает силу торможения только для задней оси; 1.0 – для передней оси; 0.5 – равномерно. В жизни, автомобили как правила имеют более высокую силу торможения передней оси, например 0.65);
        public float fHandBrakeForce { get; set; } //Параметр, отвечающий за мощность ручного тормоза транспортного средства
        public float fSteeringLock { get; set; } //Множитель максимального угла поворота руля(0.01 - 1.0)

        //Параметры тяги колес
        public float fTractionCurveMax { get; set; } // параметр, отвечающий за сцепление колес на повороте;
        public float fTractionCurveMin { get; set; } // параметр, отвечающий за пробуксовку колес;
        public float fTractionCurveLateral { get; set; } // параметр, отвечающий за боковую силу сцепления;
        public float fTractionSpringDeltaMax { get; set; } // параметр, отвечающий за расстояние от земли, при котором транспортное средство теряет сцепление;
        public float fLowSpeedTractionLossMult { get; set; } // параметр, отвечающий за силу сцепления транспортного средства при низкой скорости(значение 0.0 – исходная сила сцепления);
        public float fTractionBiasFront { get; set; } // параметр, отвечающий за распределение тяги между передними и задними колесами(от 0.01 до 0.99);
        public float fTractionLossMult { get; set; } // параметр, отвечающий за потерю сцепления шины с дорогой;

        //Параметры подвески
        public float fSuspensionForce { get; set; } // (1 / сила* количество колес) – нижний предел силы при полном выдвижении(сила подвески);
        public float fSuspensionCompDamp { get; set; } // параметр, отвечающий за силу и интенсивность вибрации подвески;
        public float fSuspensionReboundDamp { get; set; } // параметр, отвечающий за силу и интенсивность вибрации подвески на высоких скоростях;
        public float fSuspensionRaise { get; set; } // параметр, отвечающий за высоту подвески;
        public float fSuspensionBiasFront { get; set; } // параметр, отвечающий за смещение подвески вперед(большое значение подходит для грузовиков);
    }
}
