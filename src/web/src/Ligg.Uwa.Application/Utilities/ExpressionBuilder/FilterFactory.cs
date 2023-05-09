using ExpressionBuilder.Generics;
using ExpressionBuilder.Interfaces;
using System;

namespace ExpressionBuilder
{
    /// <summary>
    /// </summary>
    public static class FilterFactory
    {
        /// <summary>
        /// Creates a Filter&lt;TClass&gt; by passing the 'TClass' as a parameter.
        /// </summary>
        /// <param name="type"></param>
        /// <typeparam name="TClass"></typeparam>
        /// <returns></returns>
        public static IFilter Create(Type type)
        {
            Type[] typeArgs = { type };
            var filterType = typeof(Filter<>).MakeGenericType(typeArgs);
            return (IFilter)Activator.CreateInstance(filterType);
        }
    }
}
/*
Expression Builder
https://github.com/dbelmont/ExpressionBuilder

In short words, this library basically provides you with a simple way to create lambda expressions to filter lists and database queries by delivering an easy-to-use fluent interface that enables the creation, storage and transmission of those filters. That can be used to help to turn WebApi requests parameters into expressions, create advanced search screens with the capability to save and re-run those filters, among other things. If you would like more details on how it works, please, check out the article Build Lambda Expression Dynamically.

License
Copyright 2018 David Belmont

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at LICENSE

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
 */