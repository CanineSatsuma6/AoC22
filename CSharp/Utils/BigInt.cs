using System.Text;

namespace Utils
{
    public class BigInt
    {
        private string _value = "0";

        public BigInt()
        {
            _value = "0";
        }

        public BigInt(int val)
        {
            _value = val.ToString();
        }

        public BigInt(string val)
        {
            _value = val;
        }

        public static implicit operator BigInt(int val)
        {
            return new BigInt(val);
        }

        public static implicit operator BigInt(string val)
        {
            return new BigInt(val);
        }

        public static implicit operator string(BigInt val)
        {
            return val._value;
        }

        public static BigInt operator +(BigInt left, BigInt right)
        {
            string leftStr = left._value;
            string rightStr = right._value;
            string result = "";

            int carry = 0;

            if (leftStr.Length > rightStr.Length)
            {
                rightStr = rightStr.PadLeft(leftStr.Length, '0');
            }
            else if (rightStr.Length > leftStr.Length)
            {
                leftStr = leftStr.PadLeft(rightStr.Length, '0');
            }

            for (int i = leftStr.Length - 1; i >= 0; i--)
            {
                int leftDigit = leftStr[i] - '0';
                int rightDigit = rightStr[i] - '0';
                int sum = leftDigit + rightDigit + carry;
                string sumStr = sum.ToString();

                result = sumStr[sumStr.Length - 1] + result;

                carry = sum / 10;
            }

            if (carry > 0)
            {
                result = carry.ToString() + result;
            }

            return result.TrimStart('0');
        }

        public override string ToString()
        {
            return _value;
        }
    }
}
