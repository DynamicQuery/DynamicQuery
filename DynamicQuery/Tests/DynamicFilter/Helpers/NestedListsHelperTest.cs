﻿using Xunit;
using FluentAssertions;
using DynamicFilter.Common.Interfaces;
using DynamicFilter.Common.Helpers;
using System;

namespace DynamicQueryUnitTests
{
    public class NestedListsHelperTest
    {
        private INestedListsHelper _nestedListsHelper;

        public NestedListsHelperTest()
        {
            _nestedListsHelper = new NestedListsHelper();
        }

        #region PropertyIdsAreAtSameNestedListScopeTest

        [Theory]
        [InlineData("PersonName[Name]", "PersonName[Type]")]
        [InlineData("PersonName[Name]", "PersonName[A.B]")]
        [InlineData("PersonName[Name.B[C]]", "PersonName[Name.B[A.d]]")]
        public void PropertyIdsAreAtSameNestedListScopeTest_ShouldBeTrueForSameNestedScopePropertyIds(string propertyId1, string propertyId2)
        {
            _nestedListsHelper.PropertyIdsAreAtSameNestedListScope(propertyId1, propertyId2).Should().BeTrue();
        }

        [Theory]
        [InlineData("PersonName.Name", "PersonName.Name")]
        public void PropertyIdsAreAtSameNestedListScopeTest_ShouldBThrowExceptionWhenHaveNoLists(string propertyId1, string propertyId2)
        {
            _nestedListsHelper.Invoking(x => x.PropertyIdsAreAtSameNestedListScope(propertyId1, propertyId2))
                .Should()
                .Throw<ArgumentException>();

        }

        [Theory]
        [InlineData("Persons[PersonName.Name]", "Person[PersonName.Name]")]
        public void PropertyIdsAreAtSameNestedListScopeTest_ShouldBeFalseForDifferentScopes(string propertyId1, string propertyId2)
        {
            _nestedListsHelper.PropertyIdsAreAtSameNestedListScope(propertyId1, propertyId2).Should().BeFalse();

        }

        #endregion


        #region IsAnImmediateListTest

        [Theory]
        [InlineData("Name")]
        [InlineData("Person.Name")]
        [InlineData("Person.Names[Type]")]

        public void IsAnImmediateList_ShouldReturnFalseIfPropertyDoesNotBeginWithAList(string propertyId)
        {
            _nestedListsHelper.IsAnImmediateList(propertyId).Should().BeFalse();
        }

        [Theory]
        [InlineData("Names[Name]")]
        [InlineData("Persons[Name.Type]")]
        [InlineData("Persons[Names[Type]]")]

        public void IsAnImmediateList_ShouldReturnTrueIfPropertyBeginsWithAList(string propertyId)
        {
            _nestedListsHelper.IsAnImmediateList(propertyId).Should().BeTrue();
        }

        #endregion


        #region PropertyIdContainsListTest
        
        [Theory]
        [InlineData("Names[Type]")]
        [InlineData("PersonNames[Name.Type]")]
        [InlineData("PersonNames[Names[Type]]")]
        [InlineData("PersonName.Names[Type]")]
        [InlineData("PersonName.Names[A.Type]")]
        public void PropertyIdContainsListTest_ShouldReturnTrueIfThereExistsAListWithinIt(string propertyId)
        {
            _nestedListsHelper.PropertyIdContainsList(propertyId).Should().BeTrue();
        }

        [Theory]
        [InlineData("Name")]
        [InlineData("Name.Type")]
        [InlineData("Name.Type.Type")]
        public void PropertyIdContainsListTest_ShouldReturnFalseIfThereAreNoLists(string propertyId)
        {
            _nestedListsHelper.PropertyIdContainsList(propertyId).Should().BeFalse();
        }
        #endregion
    }
}