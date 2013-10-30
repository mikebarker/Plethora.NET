
EXPLANATION OF THE SPACIAL NAMESPACE
====================================


The key of an element in a set may be considered part of a "key space". This space may have several dimensions.

	For example, consider a set of stock prices. These may be keyed by both the stock as well as the date of the price.
	A set may contain all prices for known stocks between 1 Jan 2000 and 31 Dec 2005. Another set may contain all prices
	for a particular stock. Yet another may contain just one price for one stock.

	In this example the key space consists of two dimensions, the first dimension is the stock (or rather the stock's ID),
	and the second dimension is the date of the price. Any element within the key space is uniquely identified by the
	combination of stock ID and date.


One may consider 'Set A'-'Set B' (where - is the subtraction operator, removing all elements found in B from set A)
to consist of four possible regions. Any, all, or none of which may not be valid. We'll name these regions 'a', 'b', 'c', and 'd'.
These regions are defined as*:
    'a' is the space bound by A.minStockId (inclusive) to B.minStockId (exclusive), and A.minDate (inclusive) to A.maxDate (inclusive)
    'b' is the space bound by B.minStockId (inclusive) to B.maxStockId (inclusive), and B.maxDate (exclusive) to A.maxDate (inclusive)
    'c' is the space bound by B.minStockId (inclusive) to B.maxStockId (inclusive), and A.minDate (inclusive) to B.minDate (exclusive)
    'd' is the space bound by B.maxStockId (exclusive) to A.maxStockId (inclusive), and A.minDate (inclusive) to A.maxDate (inclusive)

The regions may be visualised as (where A is the larger bordered region, and B is the smaller):

                   A
                +---------------------+
             S  |??????@@@@@@@@@@@\\\\|
             t  |??????@@@@ b @@@@\\\\|
             o  |??????@@@@@@@@@@@\\\\|
             c  |??????+---------+\\\\|
             k  |? a ??|  B      |\ d |
                |??????|         |\\\\|
             I  |??????+---------+\\\\|
             d  |??????###########\\\\|
                |??????#### c ####\\\\|
                +---------------------+
                          Date

For each of these defined regions ('a' to 'd'), the measures derived from 'B' must be limited
by the bounds of 'A'. That is, if A.maxDate < B.minDate-1 then the region 'c' must be limited
from A.minDate to A.maxDate (and not to B.minDate-1)

Where a region's minimum (Date or StockId) exceeds it's maximum, the region is not valid
and must not be returned. e.g. Consider the case:
                     A
                +----------+
                |??????@@b@|    B
                |??????+---|-------+
                |? a ??|   |       |
                |??????+---|-------+
                |??????##c#|
                +----------+

In this case regions 'b' and 'c' are bound by the region 'A'. Note also that 'd' is not a valid
region in this case, as 'A' has no content in that area of the key-space.
