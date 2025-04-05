using UnityEngine;

namespace Game.Entities
{
    public static class ComparaisonOperatorHelpers
    {
        public static bool Compare(this ComparaisonOperator comparaisonOperator, int lhs, int rhs)
        {
            switch (comparaisonOperator)
            {
                case ComparaisonOperator.Equal:
                    {
                        return lhs == rhs;
                    }

                case ComparaisonOperator.NotEqual:
                    {
                        return lhs != rhs;
                    }

                case ComparaisonOperator.Greater:
                    {
                        return lhs > rhs;
                    }

                case ComparaisonOperator.GreaterOrEqual:
                    {
                        return lhs >= rhs;
                    }

                case ComparaisonOperator.Lesser:
                    {
                        return lhs < rhs;
                    }
                    
                case ComparaisonOperator.LesserOrEqual:
                    {
                        return lhs <= rhs;
                    }

                default:
                    {
                        Debug.LogError($"Unknown comparaison operator: {comparaisonOperator}");
                        return false;
                    }
            }
        }
    }
}