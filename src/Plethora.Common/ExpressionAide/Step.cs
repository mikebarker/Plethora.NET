namespace Plethora.ExpressionAide
{
    internal enum Direction
    {
        This,
        Left,
        Right,
        Operand,
        Object,
        Body,
        Test,
        IfTrue,
        IfFalse,
        Expression,
        NewExpression,

        Arguments,
        Expressions,
    }

    internal struct Step
    {
        #region Fields

        private readonly Direction direction;
        private readonly int index;
        #endregion

        #region Constructors

        internal Step(Direction direction)
        {
            this.direction = direction;
            this.index = -1;
        }

        internal Step(Direction direction, int index)
        {
            this.direction = direction;
            this.index = index;
        }
        #endregion

        #region Properties

        internal Direction Direction
        {
            get { return direction; }
        }

        internal int Index
        {
            get { return index; }
        }
        #endregion
    }
}
