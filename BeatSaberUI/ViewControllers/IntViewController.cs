using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatSaberUI
{
    public abstract class IntSettingsController : IncDecSettingsController
    {
        private int _value;
        protected int _min;
        protected int _max;
        protected int _increment;

        protected abstract int GetInitValue();
        protected abstract void ApplyValue(int value);
        protected abstract string TextForValue(int value);


        public override void Init()
        {
            _value = this.GetInitValue();
            this.RefreshUI();
        }
        public override void ApplySettings()
        {
            this.ApplyValue(this._value);
        }
        private void RefreshUI()
        {
            this.text = this.TextForValue(this._value);
            this.enableDec = _value > _min;
            this.enableInc = _value < _max;
        }
        public override void IncButtonPressed()
        {
            this._value += _increment;
            if (this._value > _max) this._value = _max;
            this.RefreshUI();
        }
        public override void DecButtonPressed()
        {
            this._value -= _increment;
            if (this._value < _min) this._value = _min;
            this.RefreshUI();
        }
    }

    public class IntViewController : IntSettingsController
    {
        public delegate int GetInt();
        public event GetInt GetValue;

        public delegate void SetInt(int value);
        public event SetInt SetValue;

        public void SetValues(int min, int max, int increment)
        {
            _min = min;
            _max = max;
            _increment = increment;
        }

        public void UpdateIncrement(int increment)
        {
            _increment = increment;
        }

        private int FixValue(int value)
        {
            if (value % _increment != 0)
            {
                value -= (value % _increment);
            }
            if (value > _max) value = _max;
            if (value < _min) value = _min;
            return value;
        }

        protected override int GetInitValue()
        {
            int value = 0;
            if (GetValue != null)
            {
                value = FixValue(GetValue());
            }
            return value;
        }

        protected override void ApplyValue(int value)
        {
            if (SetValue != null)
            {
                SetValue(FixValue(value));
            }
        }

        protected override string TextForValue(int value)
        {
            return value.ToString();
        }
    }
}
