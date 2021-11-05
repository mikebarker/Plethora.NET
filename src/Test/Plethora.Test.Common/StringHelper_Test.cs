using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Plethora.Test
{
    [TestClass]
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

        [TestMethod]
        public void WhiteSpace()
        {
            // Action
            char[] whiteSpace = StringHelper.WhiteSpace;

            // Assert
            Assert.IsNotNull(whiteSpace);
            var spaceIndex = Array.IndexOf(whiteSpace, ' ');
            Assert.IsTrue(spaceIndex >= 0);
        }
        #endregion

        #region IndexNotOfAny

        [TestMethod]
        public void IndexNotOfAny()
        {
            // Action
            int index = s.IndexNotOfAny(notOfAny);

            // Assert
            Assert.AreEqual(8, index);
        }

        [TestMethod]
        public void IndexNotOfAny_NotFound()
        {
            // Action
            int index = s.IndexNotOfAny(s.ToCharArray());

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void IndexNotOfAny_Start()
        {
            // Action
            int index = s.IndexNotOfAny(notOfAny, 4);

            // Assert
            Assert.AreEqual(8, index);
        }

        [TestMethod]
        public void IndexNotOfAny_Start_NotFound()
        {
            // Action
            int index = s.IndexNotOfAny(s.ToCharArray(), 4);

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void IndexNotOfAny_Start_Count()
        {
            // Action
            int index = s.IndexNotOfAny(notOfAny, 4, 9);

            // Assert
            Assert.AreEqual(8, index);
        }

        [TestMethod]
        public void IndexNotOfAny_Start_Count_NotFound()
        {
            // Action
            int index = s.IndexNotOfAny(notOfAny, 4, 3);

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void IndexNotOfAny_Fail_Null()
        {
            try
            {
                // Action
                ((string)null).IndexNotOfAny(notOfAny);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void IndexNotOfAny_Fail_StartNegative()
        {
            try
            {
                // Action
                s.IndexNotOfAny(notOfAny, -5);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void IndexNotOfAny_Fail_CountNegative()
        {
            try
            {
                // Action
                s.IndexNotOfAny(notOfAny, 4, -2);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void IndexNotOfAny_Fail_StartPlusCountTooLarge()
        {
            try
            {
                // Action
                s.IndexNotOfAny(notOfAny, 17, 22);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
        #endregion

        #region LastIndexNotOfAny

        [TestMethod]
        public void LastIndexNotOfAny()
        {
            // Action
            int index = s.LastIndexNotOfAny(notOfAny);

            // Assert
            Assert.AreEqual(17, index);
        }

        [TestMethod]
        public void LastIndexNotOfAny_NotFound()
        {
            // Action
            int index = s.LastIndexNotOfAny(s.ToCharArray());

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void LastIndexNotOfAny_Start()
        {
            // Action
            int index = s.LastIndexNotOfAny(notOfAny, 15);

            // Assert
            Assert.AreEqual(11, index);
        }

        [TestMethod]
        public void LastIndexNotOfAny_Start_NotFound()
        {
            // Action
            int index = s.LastIndexNotOfAny(s.ToCharArray(), 4);

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void LastIndexNotOfAny_Start_Count()
        {
            // Action
            int index = s.LastIndexNotOfAny(notOfAny, 15, 9);

            // Assert
            Assert.AreEqual(11, index);
        }

        [TestMethod]
        public void LastIndexNotOfAny_Start_Count_NotFound()
        {
            // Action
            int index = s.LastIndexNotOfAny(notOfAny, 4, 3);

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void LastIndexNotOfAny_Fail_Null()
        {
            try
            {
                // Action
                ((string)null).LastIndexNotOfAny(notOfAny);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void LastIndexNotOfAny_Fail_StartNegative()
        {
            try
            {
                // Action
                s.LastIndexNotOfAny(notOfAny, -5);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void LastIndexNotOfAny_Fail_CountNegative()
        {
            try
            {
                // Action
                s.LastIndexNotOfAny(notOfAny, 4, -2);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void LastIndexNotOfAny_Fail_StartPlusCountTooLarge()
        {
            try
            {
                // Action
                s.IndexNotOfAny(notOfAny, 17, 22);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
        #endregion

        #region IndexOfWhiteSpace

        [TestMethod]
        public void IndexOfWhiteSpace()
        {
            // Action
            int index = s.IndexOfWhiteSpace();

            // Assert
            Assert.AreEqual(4, index);
        }

        [TestMethod]
        public void IndexOfWhiteSpace_NotFound()
        {
            // Action
            int index = "ThereIsNoWhiteSpace".IndexOfWhiteSpace();

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void IndexOfWhiteSpace_StartIndex()
        {
            // Action
            int index = s.IndexOfWhiteSpace(11);

            // Assert
            Assert.AreEqual(14, index);
        }

        [TestMethod]
        public void IndexOfWhiteSpace_StartIndex_NotFound()
        {
            // Action
            int index = s.IndexOfWhiteSpace(16);

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void IndexOfWhiteSpace_StartIndex_Count()
        {
            // Action
            int index = s.IndexOfWhiteSpace(11, 5);

            // Assert
            Assert.AreEqual(14, index);
        }

        [TestMethod]
        public void IndexOfWhiteSpace_StartIndex_Count_NotFound()
        {
            // Action
            int index = s.IndexOfWhiteSpace(11, 2);

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void IndexOfWhiteSpace_Fail_Null()
        {
            try
            {
                // Action
                ((string)null).IndexOfWhiteSpace();
    
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void IndexOfWhiteSpace_Fail_StartNegative()
        {
            try
            {
                // Action
                s.IndexOfWhiteSpace(-5);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void IndexOfWhiteSpace_Fail_CountNegative()
        {
            try
            {
                // Action
                s.IndexOfWhiteSpace(0, -5);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void IndexOfWhiteSpace_Fail_StartPlusCountTooLarge()
        {
            try
            {
                // Action
                s.IndexOfWhiteSpace(16, 22);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
        #endregion

        #region LastIndexOfWhiteSpace

        [TestMethod]
        public void LastIndexOfWhiteSpace()
        {
            // Action
            int index = s.LastIndexOfWhiteSpace();

            // Assert
            Assert.AreEqual(14, index);
        }

        [TestMethod]
        public void LastIndexOfWhiteSpace_NotFound()
        {
            // Action
            int index = "ThereIsNoWhiteSpace".LastIndexOfWhiteSpace();

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void LastIndexOfWhiteSpace_StartIndex()
        {
            // Action
            int index = s.LastIndexOfWhiteSpace(11);

            // Assert
            Assert.AreEqual(9, index);
        }

        [TestMethod]
        public void LastIndexOfWhiteSpace_StartIndex_NotFound()
        {
            // Action
            int index = s.LastIndexOfWhiteSpace(3);

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void LastIndexOfWhiteSpace_StartIndex_Count()
        {
            // Action
            int index = s.LastIndexOfWhiteSpace(11, 5);

            // Assert
            Assert.AreEqual(9, index);
        }

        [TestMethod]
        public void LastIndexOfWhiteSpace_StartIndex_Count_NotFound()
        {
            // Action
            int index = s.LastIndexOfWhiteSpace(11, 2);

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void LastIndexOfWhiteSpace_Fail_Null()
        {
            try
            {
                // Action
                ((string)null).LastIndexOfWhiteSpace();
    
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void LastIndexOfWhiteSpace_Fail_StartNegative()
        {
            try
            {
                // Action
                s.LastIndexOfWhiteSpace(-5);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void LastIndexOfWhiteSpace_Fail_CountNegative()
        {
            try
            {
                // Action
                s.LastIndexOfWhiteSpace(0, -5);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void LastIndexOfWhiteSpace_Fail_StartPlusCountTooLarge()
        {
            try
            {
                // Action
                s.LastIndexOfWhiteSpace(16, 22);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
        #endregion

        #region IndexNotOfWhiteSpace

        [TestMethod]
        public void IndexNotOfWhiteSpace()
        {
            // Action
            int index = lotsOfWhiteSpace.IndexNotOfWhiteSpace();

            // Assert
            Assert.AreEqual(4, index);
        }

        [TestMethod]
        public void IndexNotOfWhiteSpace_NotFound()
        {
            // Action
            int index = "  \t  ".IndexNotOfWhiteSpace();

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void IndexNotOfWhiteSpace_StartIndex()
        {
            // Action
            int index = lotsOfWhiteSpace.IndexNotOfWhiteSpace(19);

            // Assert
            Assert.AreEqual(22, index);
        }

        [TestMethod]
        public void IndexNotOfWhiteSpace_StartIndex_NotFound()
        {
            // Action
            int index = lotsOfWhiteSpace.IndexNotOfWhiteSpace(30);

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void IndexNotOfWhiteSpace_StartIndex_Count()
        {
            // Action
            int index = lotsOfWhiteSpace.IndexNotOfWhiteSpace(19, 5);

            // Assert
            Assert.AreEqual(22, index);
        }

        [TestMethod]
        public void IndexNotOfWhiteSpace_StartIndex_Count_NotFound()
        {
            // Action
            int index = lotsOfWhiteSpace.IndexNotOfWhiteSpace(19, 2);

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void IndexNotOfWhiteSpace_Fail_Null()
        {
            try
            {
                // Action
                ((string)null).IndexNotOfWhiteSpace();
    
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void IndexNotOfWhiteSpace_Fail_StartNegative()
        {
            try
            {
                // Action
                lotsOfWhiteSpace.IndexNotOfWhiteSpace(-5);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void IndexNotOfWhiteSpace_Fail_CountNegative()
        {
            try
            {
                // Action
                lotsOfWhiteSpace.IndexNotOfWhiteSpace(0, -5);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void IndexNotOfWhiteSpace_Fail_StartPlusCountTooLarge()
        {
            try
            {
                // Action
                lotsOfWhiteSpace.IndexNotOfWhiteSpace(16, 22);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
        #endregion

        #region LastIndexNotOfWhiteSpace

        [TestMethod]
        public void LastIndexNotOfWhiteSpace()
        {
            // Action
            int index = lotsOfWhiteSpace.LastIndexNotOfWhiteSpace();

            // Assert
            Assert.AreEqual(27, index);
        }

        [TestMethod]
        public void LastIndexNotOfWhiteSpace_NotFound()
        {
            // Action
            int index = "   \t   ".LastIndexNotOfWhiteSpace();

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void LastIndexNotOfWhiteSpace_StartIndex()
        {
            // Action
            int index = lotsOfWhiteSpace.LastIndexNotOfWhiteSpace(21);

            // Assert
            Assert.AreEqual(18, index);
        }

        [TestMethod]
        public void LastIndexNotOfWhiteSpace_StartIndex_NotFound()
        {
            // Action
            int index = lotsOfWhiteSpace.LastIndexNotOfWhiteSpace(3);

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void LastIndexNotOfWhiteSpace_StartIndex_Count()
        {
            // Action
            int index = lotsOfWhiteSpace.LastIndexNotOfWhiteSpace(21, 5);

            // Assert
            Assert.AreEqual(18, index);
        }

        [TestMethod]
        public void LastIndexNotOfWhiteSpace_StartIndex_Count_NotFound()
        {
            // Action
            int index = lotsOfWhiteSpace.LastIndexNotOfWhiteSpace(21, 2);

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void LastIndexNotOfWhiteSpace_Fail_Null()
        {
            try
            {
                // Action
                ((string)null).LastIndexNotOfWhiteSpace();
    
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void LastIndexNotOfWhiteSpace_Fail_StartNegative()
        {
            try
            {
                // Action
                lotsOfWhiteSpace.LastIndexNotOfWhiteSpace(-5);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void LastIndexNotOfWhiteSpace_Fail_CountNegative()
        {
            try
            {
                // Action
                lotsOfWhiteSpace.LastIndexNotOfWhiteSpace(0, -5);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void LastIndexNotOfWhiteSpace_Fail_StartPlusCountTooLarge()
        {
            try
            {
                // Action
                lotsOfWhiteSpace.LastIndexNotOfWhiteSpace(16, 22);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
        #endregion

        #region IndexOfWord

        [TestMethod]
        public void IndexOfWord()
        {
            // Action
            int index = s.IndexOfWord();

            // Assert
            Assert.AreEqual(1, index);
        }

        [TestMethod]
        public void IndexOfWord_NotFound()
        {
            // Action
            int index = "    ".IndexOfWord();

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void IndexOfWord_NotLeading()
        {
            // Action
            int index = lotsOfWhiteSpace.IndexOfWord();

            // Assert
            Assert.AreEqual(4, index);
        }

        [TestMethod]
        public void IndexOfWord_Start()
        {
            // Action
            int index = s.IndexOfWord(11);

            // Assert
            Assert.AreEqual(15, index);
        }

        [TestMethod]
        public void IndexOfWord_Start_NotFound()
        {
            // Action
            int index = s.IndexOfWord(19);

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void IndexOfWord_StartBeginning()
        {
            // Action
            int index = s.IndexOfWord(11, true);

            // Assert
            Assert.AreEqual(15, index);
        }

        [TestMethod]
        public void IndexOfWord_StartBeginning_NotFound()
        {
            // Action
            int index = s.IndexOfWord(19, true);

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void IndexOfWord_StartEnd()
        {
            // Action
            int index = s.IndexOfWord(11, false);

            // Assert
            Assert.AreEqual(13, index);
        }

        [TestMethod]
        public void IndexOfWord_StartEnd_NotFound()
        {
            // Action
            int index = "     ".IndexOfWord(2, false);

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void IndexOfWord_Fail_Null()
        {
            try
            {
                // Action
                ((string)null).IndexOfWord();

                Assert.Fail();
            }
            catch(ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void IndexOfWord_Fail_StartNegative()
        {
            try
            {
                // Action
                s.IndexOfWord(-5);

                Assert.Fail();
            }
            catch(ArgumentOutOfRangeException)
            {
            }
        }
        #endregion

        #region LastIndexOfWord

        [TestMethod]
        public void LastIndexOfWord()
        {
            // Action
            int index = s.LastIndexOfWord();

            // Assert
            Assert.AreEqual(15, index);
        }

        [TestMethod]
        public void LastIndexOfWord_NotFound()
        {
            // Action
            int index = "    ".LastIndexOfWord();

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void LastIndexOfWord_Start()
        {
            // Action
            int index = s.LastIndexOfWord(11);

            // Assert
            Assert.AreEqual(10, index);
        }

        [TestMethod]
        public void LastIndexOfWord_Start_NotFound()
        {
            // Action
            int index = "    this".LastIndexOfWord(3);

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void LastIndexOfWord_StartBeginning()
        {
            // Action
            int index = s.LastIndexOfWord(12, true);

            // Assert
            Assert.AreEqual(10, index);
        }

        [TestMethod]
        public void LastIndexOfWord_StartBeginning_NotFound()
        {
            // Action
            int index = "    this".LastIndexOfWord(3, true);

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void LastIndexOfWord_StartEnd_NotEndOfWord()
        {
            // Action
            int index = s.LastIndexOfWord(11, false);

            // Assert
            Assert.AreEqual(8, index);
        }

        [TestMethod]
        public void LastIndexOfWord_StartEnd_EndOfWord()
        {
            // Action
            int index = s.LastIndexOfWord(13, false);

            // Assert
            Assert.AreEqual(13, index);
        }

        [TestMethod]
        public void LastIndexOfWord_StartEnd_NotFound()
        {
            // Action
            int index = "     ".LastIndexOfWord(2, false);

            // Assert
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void LastIndexOfWord_Fail_Null()
        {
            try
            {
                // Action
                ((string)null).LastIndexOfWord();

                Assert.Fail();
            }
            catch(ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void LastIndexOfWord_Fail_StartNegative()
        {
            try
            {
                // Action
                s.LastIndexOfWord(-5);

                Assert.Fail();
            }
            catch(ArgumentOutOfRangeException)
            {
            }
        }
        #endregion
    }
}
