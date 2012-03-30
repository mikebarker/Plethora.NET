using System;
using NUnit.Framework;

namespace Plethora.Test
{
    [TestFixture]
    public class StringHelper_Test
    {
        //                        000000000011111111112222222222
        //                        012345678901234567890123456789
        private const string s = "This is a test string.";

        //                                       0 0000000001111111111 2222222222333333
        //                                       0 1234567890123456789 0123456789012345
        private const string lotsOfWhiteSpace = " \t  lots  of  white \t space.     ";

        //For the variable s above:
        // the first character not in this set should be 'a'.
        // the last character not in this set should be 'r'.
        private readonly char[] notOfAny = new[] { 'T', 'h', 'i', 's', ' ', 't', 'n', 'g', '.' };

        #region WhiteSpace

        [Test]
        public void WhiteSpace()
        {
            //exec
            char[] whiteSpace = StringHelper.WhiteSpace;

            //test
            Assert.IsNotNull(whiteSpace);
            var spaceIndex = Array.IndexOf(whiteSpace, ' ');
            Assert.IsTrue(spaceIndex >= 0);
        }
        #endregion

        #region IndexNotOfAny

        [Test]
        public void IndexNotOfAny()
        {
            //exec
            int index = s.IndexNotOfAny(notOfAny);

            //test
            Assert.AreEqual(8, index);
        }

        [Test]
        public void IndexNotOfAny_NotFound()
        {
            //exec
            int index = s.IndexNotOfAny(s.ToCharArray());

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void IndexNotOfAny_Start()
        {
            //exec
            int index = s.IndexNotOfAny(notOfAny, 4);

            //test
            Assert.AreEqual(8, index);
        }

        [Test]
        public void IndexNotOfAny_Start_NotFound()
        {
            //exec
            int index = s.IndexNotOfAny(s.ToCharArray(), 4);

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void IndexNotOfAny_Start_Count()
        {
            //exec
            int index = s.IndexNotOfAny(notOfAny, 4, 9);

            //test
            Assert.AreEqual(8, index);
        }

        [Test]
        public void IndexNotOfAny_Start_Count_NotFound()
        {
            //exec
            int index = s.IndexNotOfAny(notOfAny, 4, 3);

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void IndexNotOfAny_Fail_Null()
        {
            try
            {
                //exec
                ((string)null).IndexNotOfAny(notOfAny);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void IndexNotOfAny_Fail_StartNegative()
        {
            try
            {
                //exec
                s.IndexNotOfAny(notOfAny, -5);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void IndexNotOfAny_Fail_CountNegative()
        {
            try
            {
                //exec
                s.IndexNotOfAny(notOfAny, 4, -2);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void IndexNotOfAny_Fail_StartPlusCountTooLarge()
        {
            try
            {
                //exec
                s.IndexNotOfAny(notOfAny, 17, 22);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion

        #region LastIndexNotOfAny

        [Test]
        public void LastIndexNotOfAny()
        {
            //exec
            int index = s.LastIndexNotOfAny(notOfAny);

            //test
            Assert.AreEqual(17, index);
        }

        [Test]
        public void LastIndexNotOfAny_NotFound()
        {
            //exec
            int index = s.LastIndexNotOfAny(s.ToCharArray());

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void LastIndexNotOfAny_Start()
        {
            //exec
            int index = s.LastIndexNotOfAny(notOfAny, 15);

            //test
            Assert.AreEqual(11, index);
        }

        [Test]
        public void LastIndexNotOfAny_Start_NotFound()
        {
            //exec
            int index = s.LastIndexNotOfAny(s.ToCharArray(), 4);

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void LastIndexNotOfAny_Start_Count()
        {
            //exec
            int index = s.LastIndexNotOfAny(notOfAny, 15, 9);

            //test
            Assert.AreEqual(11, index);
        }

        [Test]
        public void LastIndexNotOfAny_Start_Count_NotFound()
        {
            //exec
            int index = s.LastIndexNotOfAny(notOfAny, 4, 3);

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void LastIndexNotOfAny_Fail_Null()
        {
            try
            {
                //exec
                ((string)null).LastIndexNotOfAny(notOfAny);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void LastIndexNotOfAny_Fail_StartNegative()
        {
            try
            {
                //exec
                s.LastIndexNotOfAny(notOfAny, -5);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void LastIndexNotOfAny_Fail_CountNegative()
        {
            try
            {
                //exec
                s.LastIndexNotOfAny(notOfAny, 4, -2);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void LastIndexNotOfAny_Fail_StartPlusCountTooLarge()
        {
            try
            {
                //exec
                s.IndexNotOfAny(notOfAny, 17, 22);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion

        #region IndexOfWhiteSpace

        [Test]
        public void IndexOfWhiteSpace()
        {
            //exec
            int index = s.IndexOfWhiteSpace();

            //test
            Assert.AreEqual(4, index);
        }

        [Test]
        public void IndexOfWhiteSpace_NotFound()
        {
            //exec
            int index = "ThereIsNoWhiteSpace".IndexOfWhiteSpace();

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void IndexOfWhiteSpace_StartIndex()
        {
            //exec
            int index = s.IndexOfWhiteSpace(11);

            //test
            Assert.AreEqual(14, index);
        }

        [Test]
        public void IndexOfWhiteSpace_StartIndex_NotFound()
        {
            //exec
            int index = s.IndexOfWhiteSpace(16);

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void IndexOfWhiteSpace_StartIndex_Count()
        {
            //exec
            int index = s.IndexOfWhiteSpace(11, 5);

            //test
            Assert.AreEqual(14, index);
        }

        [Test]
        public void IndexOfWhiteSpace_StartIndex_Count_NotFound()
        {
            //exec
            int index = s.IndexOfWhiteSpace(11, 2);

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void IndexOfWhiteSpace_Fail_Null()
        {
            try
            {
                //exec
                ((string)null).IndexOfWhiteSpace();
    
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void IndexOfWhiteSpace_Fail_StartNegative()
        {
            try
            {
                //exec
                s.IndexOfWhiteSpace(-5);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void IndexOfWhiteSpace_Fail_CountNegative()
        {
            try
            {
                //exec
                s.IndexOfWhiteSpace(0, -5);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void IndexOfWhiteSpace_Fail_StartPlusCountTooLarge()
        {
            try
            {
                //exec
                s.IndexOfWhiteSpace(16, 22);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion

        #region LastIndexOfWhiteSpace

        [Test]
        public void LastIndexOfWhiteSpace()
        {
            //exec
            int index = s.LastIndexOfWhiteSpace();

            //test
            Assert.AreEqual(14, index);
        }

        [Test]
        public void LastIndexOfWhiteSpace_NotFound()
        {
            //exec
            int index = "ThereIsNoWhiteSpace".LastIndexOfWhiteSpace();

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void LastIndexOfWhiteSpace_StartIndex()
        {
            //exec
            int index = s.LastIndexOfWhiteSpace(11);

            //test
            Assert.AreEqual(9, index);
        }

        [Test]
        public void LastIndexOfWhiteSpace_StartIndex_NotFound()
        {
            //exec
            int index = s.LastIndexOfWhiteSpace(3);

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void LastIndexOfWhiteSpace_StartIndex_Count()
        {
            //exec
            int index = s.LastIndexOfWhiteSpace(11, 5);

            //test
            Assert.AreEqual(9, index);
        }

        [Test]
        public void LastIndexOfWhiteSpace_StartIndex_Count_NotFound()
        {
            //exec
            int index = s.LastIndexOfWhiteSpace(11, 2);

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void LastIndexOfWhiteSpace_Fail_Null()
        {
            try
            {
                //exec
                ((string)null).LastIndexOfWhiteSpace();
    
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void LastIndexOfWhiteSpace_Fail_StartNegative()
        {
            try
            {
                //exec
                s.LastIndexOfWhiteSpace(-5);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void LastIndexOfWhiteSpace_Fail_CountNegative()
        {
            try
            {
                //exec
                s.LastIndexOfWhiteSpace(0, -5);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void LastIndexOfWhiteSpace_Fail_StartPlusCountTooLarge()
        {
            try
            {
                //exec
                s.LastIndexOfWhiteSpace(16, 22);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion

        #region IndexNotOfWhiteSpace

        [Test]
        public void IndexNotOfWhiteSpace()
        {
            //exec
            int index = lotsOfWhiteSpace.IndexNotOfWhiteSpace();

            //test
            Assert.AreEqual(4, index);
        }

        [Test]
        public void IndexNotOfWhiteSpace_NotFound()
        {
            //exec
            int index = "  \t  ".IndexNotOfWhiteSpace();

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void IndexNotOfWhiteSpace_StartIndex()
        {
            //exec
            int index = lotsOfWhiteSpace.IndexNotOfWhiteSpace(19);

            //test
            Assert.AreEqual(22, index);
        }

        [Test]
        public void IndexNotOfWhiteSpace_StartIndex_NotFound()
        {
            //exec
            int index = lotsOfWhiteSpace.IndexNotOfWhiteSpace(30);

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void IndexNotOfWhiteSpace_StartIndex_Count()
        {
            //exec
            int index = lotsOfWhiteSpace.IndexNotOfWhiteSpace(19, 5);

            //test
            Assert.AreEqual(22, index);
        }

        [Test]
        public void IndexNotOfWhiteSpace_StartIndex_Count_NotFound()
        {
            //exec
            int index = lotsOfWhiteSpace.IndexNotOfWhiteSpace(19, 2);

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void IndexNotOfWhiteSpace_Fail_Null()
        {
            try
            {
                //exec
                ((string)null).IndexNotOfWhiteSpace();
    
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void IndexNotOfWhiteSpace_Fail_StartNegative()
        {
            try
            {
                //exec
                lotsOfWhiteSpace.IndexNotOfWhiteSpace(-5);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void IndexNotOfWhiteSpace_Fail_CountNegative()
        {
            try
            {
                //exec
                lotsOfWhiteSpace.IndexNotOfWhiteSpace(0, -5);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void IndexNotOfWhiteSpace_Fail_StartPlusCountTooLarge()
        {
            try
            {
                //exec
                lotsOfWhiteSpace.IndexNotOfWhiteSpace(16, 22);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion

        #region LastIndexNotOfWhiteSpace

        [Test]
        public void LastIndexNotOfWhiteSpace()
        {
            //exec
            int index = lotsOfWhiteSpace.LastIndexNotOfWhiteSpace();

            //test
            Assert.AreEqual(27, index);
        }

        [Test]
        public void LastIndexNotOfWhiteSpace_NotFound()
        {
            //exec
            int index = "   \t   ".LastIndexNotOfWhiteSpace();

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void LastIndexNotOfWhiteSpace_StartIndex()
        {
            //exec
            int index = lotsOfWhiteSpace.LastIndexNotOfWhiteSpace(21);

            //test
            Assert.AreEqual(18, index);
        }

        [Test]
        public void LastIndexNotOfWhiteSpace_StartIndex_NotFound()
        {
            //exec
            int index = lotsOfWhiteSpace.LastIndexNotOfWhiteSpace(3);

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void LastIndexNotOfWhiteSpace_StartIndex_Count()
        {
            //exec
            int index = lotsOfWhiteSpace.LastIndexNotOfWhiteSpace(21, 5);

            //test
            Assert.AreEqual(18, index);
        }

        [Test]
        public void LastIndexNotOfWhiteSpace_StartIndex_Count_NotFound()
        {
            //exec
            int index = lotsOfWhiteSpace.LastIndexNotOfWhiteSpace(21, 2);

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void LastIndexNotOfWhiteSpace_Fail_Null()
        {
            try
            {
                //exec
                ((string)null).LastIndexNotOfWhiteSpace();
    
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void LastIndexNotOfWhiteSpace_Fail_StartNegative()
        {
            try
            {
                //exec
                lotsOfWhiteSpace.LastIndexNotOfWhiteSpace(-5);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void LastIndexNotOfWhiteSpace_Fail_CountNegative()
        {
            try
            {
                //exec
                lotsOfWhiteSpace.LastIndexNotOfWhiteSpace(0, -5);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void LastIndexNotOfWhiteSpace_Fail_StartPlusCountTooLarge()
        {
            try
            {
                //exec
                lotsOfWhiteSpace.LastIndexNotOfWhiteSpace(16, 22);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion

        #region IndexOfWord

        [Test]
        public void IndexOfWord()
        {
            //exec
            int index = s.IndexOfWord();

            //test
            Assert.AreEqual(1, index);
        }

        [Test]
        public void IndexOfWord_NotFound()
        {
            //exec
            int index = "    ".IndexOfWord();

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void IndexOfWord_NotLeading()
        {
            //exec
            int index = lotsOfWhiteSpace.IndexOfWord();

            //test
            Assert.AreEqual(4, index);
        }

        [Test]
        public void IndexOfWord_Start()
        {
            //exec
            int index = s.IndexOfWord(11);

            //test
            Assert.AreEqual(15, index);
        }

        [Test]
        public void IndexOfWord_Start_NotFound()
        {
            //exec
            int index = s.IndexOfWord(19);

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void IndexOfWord_StartBeginning()
        {
            //exec
            int index = s.IndexOfWord(11, true);

            //test
            Assert.AreEqual(15, index);
        }

        [Test]
        public void IndexOfWord_StartBeginning_NotFound()
        {
            //exec
            int index = s.IndexOfWord(19, true);

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void IndexOfWord_StartEnd()
        {
            //exec
            int index = s.IndexOfWord(11, false);

            //test
            Assert.AreEqual(13, index);
        }

        [Test]
        public void IndexOfWord_StartEnd_NotFound()
        {
            //exec
            int index = "     ".IndexOfWord(2, false);

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void IndexOfWord_Fail_Null()
        {
            try
            {
                //exec
                ((string)null).IndexOfWord();

                Assert.Fail();
            }
            catch(ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void IndexOfWord_Fail_StartNegative()
        {
            try
            {
                //exec
                s.IndexOfWord(-5);

                Assert.Fail();
            }
            catch(ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion

        #region LastIndexOfWord

        [Test]
        public void LastIndexOfWord()
        {
            //exec
            int index = s.LastIndexOfWord();

            //test
            Assert.AreEqual(15, index);
        }

        [Test]
        public void LastIndexOfWord_NotFound()
        {
            //exec
            int index = "    ".LastIndexOfWord();

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void LastIndexOfWord_Start()
        {
            //exec
            int index = s.LastIndexOfWord(11);

            //test
            Assert.AreEqual(10, index);
        }

        [Test]
        public void LastIndexOfWord_Start_NotFound()
        {
            //exec
            int index = "    this".LastIndexOfWord(3);

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void LastIndexOfWord_StartBeginning()
        {
            //exec
            int index = s.LastIndexOfWord(12, true);

            //test
            Assert.AreEqual(10, index);
        }

        [Test]
        public void LastIndexOfWord_StartBeginning_NotFound()
        {
            //exec
            int index = "    this".LastIndexOfWord(3, true);

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void LastIndexOfWord_StartEnd_NotEndOfWord()
        {
            //exec
            int index = s.LastIndexOfWord(11, false);

            //test
            Assert.AreEqual(8, index);
        }

        [Test]
        public void LastIndexOfWord_StartEnd_EndOfWord()
        {
            //exec
            int index = s.LastIndexOfWord(13, false);

            //test
            Assert.AreEqual(13, index);
        }

        [Test]
        public void LastIndexOfWord_StartEnd_NotFound()
        {
            //exec
            int index = "     ".LastIndexOfWord(2, false);

            //test
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void LastIndexOfWord_Fail_Null()
        {
            try
            {
                //exec
                ((string)null).LastIndexOfWord();

                Assert.Fail();
            }
            catch(ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void LastIndexOfWord_Fail_StartNegative()
        {
            try
            {
                //exec
                s.LastIndexOfWord(-5);

                Assert.Fail();
            }
            catch(ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion
    }
}
