namespace PagedData_Specification
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using OpenRasta.Data;
    using OpenRasta.Extensions;
    using OpenRasta.Testing.Specifications;

    #endregion

    [TestFixture]
    public class when_selecting_pages : context
    {
        readonly IQueryable<int> rangeOfValues = Enumerable.Range(1, 20).AsQueryable();

        [Test]
        public void asking_for_a_page_when_it_doesnt_exist_because_there_are_not_enough_results_will_throw_an_exception()
        {
            Executing(() => rangeOfValues.SelectPagedData(3, 10, null))
                .ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void asking_for_an_invalid_page_number_raises_an_exception()
        {
            var listToQuery = Enumerable.Range(1, 20).AsQueryable();

            Executing(() => rangeOfValues.SelectPagedData(0, 10, null)).ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void asking_for_an_invalid_page_size_raises_an_exception()
        {
            Executing(() => rangeOfValues.SelectPagedData(0, 0, null)).ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void requesting_page_1_is_a_valida_action_even_when_tehre_are_no_results()
        {
            var page = new List<int>().AsQueryable().SelectPagedData(1, 10);

            page.CurrentPage.ShouldBe(1);
        }

        [Test]
        public void requesting_the_second_page_for_a_page_size_of_5_returns_5_items_and_4_pages()
        {
            var page = rangeOfValues.SelectPagedData(2, 5, null);

            page.Items.Count.ShouldBe(5);
            page.CurrentPage.ShouldBe(2);
            page.OtherPages.Count.ShouldBe(3);
        }
 
        [Test]
        public void there_are_two_pages_when_the_page_count_is_19()
        {
            var page = rangeOfValues.SelectPagedData(1, 19);
            
            page.Items.Count.ShouldBe(19);
            page.CurrentPage.ShouldBe(1);
            page.OtherPages.Count.ShouldBe(1);
        }
    }
}