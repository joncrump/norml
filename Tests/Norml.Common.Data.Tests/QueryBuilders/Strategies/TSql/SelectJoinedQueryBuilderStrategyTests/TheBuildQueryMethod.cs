﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq.Expressions;

using Moq;
using NUnit.Framework;
using Norml.Common.Data.Attributes;
using Norml.Common.Data.QueryBuilders;
using Norml.Common.Data.QueryBuilders.Strategies;
using Norml.Common.Data.QueryBuilders.Strategies.TSql;
using Norml.Common.Extensions;
using Norml.Tests.Common.Base;

namespace Norml.Common.Data.Tests.QueryBuilders.Strategies.TSql.SelectJoinedQueryBuilderStrategyTests
{
    [TestFixture]
    public class TheBuildQueryMethod : MockTestBase<SelectJoinedQueryBuilderStrategy>
    {
        protected override void Setup()
        {
            base.Setup();

            Mocks.Get<IQueryBuilderStrategyFactory>()
                .Setup(x => x.GetBuilderStrategy(QueryKind.SelectSingleTable))
                .Returns(new Mock<IQueryBuilderStrategy>().Object);

            Mocks.Get<IPredicateBuilder>()
                .Setup(x => x.BuildContainer(It.IsAny<Expression>(), It.IsAny<Type>(),
                    It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new QueryContainer(DataGenerator.GenerateString()));
        }

        [Test]
        public void WillInvokeQueryBuilderStrategyFactoryIfTypeHasNoJoinAttribute()
        {
            dynamic parameters = new ExpandoObject();
            Expression<Func<NoJoinAttributeClass, bool>> predicate = p => p.IsNotNull();

            parameters.Predicate = predicate;
            parameters.CanDirtyRead = true;
            parameters.IncludeParameters = true;
            parameters.DesiredFields = null;
            parameters.TableName = null;

            SystemUnderTest.BuildQuery<NoJoinAttributeClass>(parameters);

            Mocks.Get<IQueryBuilderStrategyFactory>()
                .Verify(x => x.GetBuilderStrategy(QueryKind.SelectSingleTable), Times.Once);
        }

        [Test]
        public void WillReturnQueryInfoIfTypeHasNoJoinAttributes()
        {
            QueryInfo expected = null;

            expected = ObjectCreator.CreateNew<QueryInfo>();
            var strategy = new Mock<IQueryBuilderStrategy>();

            strategy
                .Setup(x => x.BuildQuery<NoJoinAttributeClass>(It.IsAny<object>()))
                .Returns(expected);

            Mocks.Get<IQueryBuilderStrategyFactory>()
                .Setup(x => x.GetBuilderStrategy(QueryKind.SelectSingleTable))
                .Returns(strategy.Object);

            dynamic parameters = new ExpandoObject();
            Expression<Func<NoJoinAttributeClass, bool>> predicate = p => p.IsNotNull();

            parameters.Predicate = predicate;
            parameters.CanDirtyRead = true;
            parameters.IncludeParameters = true;
            parameters.DesiredFields = null;
            parameters.TableName = null;

            var actual = SystemUnderTest.BuildQuery<NoJoinAttributeClass>(parameters);

            Asserter.AssertEquality(expected, actual, new[]
            {
                "Parameters", "tableObjectMappings"
            });
        }

        [Test]
        public void WillNotInvokeQueryBuilderStrategyFactoryIfTypeHasJoinAttributes()
        {
            var fieldHelper = Mocks.Get<IFieldHelper>();

            fieldHelper
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(), It.IsAny<ParentClass>(), It.IsAny<bool>(),
                    It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = "dbo.Parent",
                    Alias = "t1",
                    FieldMappings = new Dictionary<string, FieldParameterMapping>
                                        {
                                            {DataGenerator.GenerateString(), ObjectCreator.CreateNew<FieldParameterMapping>()}
                                        }
                });

            fieldHelper
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(), It.IsAny<Child1>(), It.IsAny<bool>(),
                    It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = "dbo.Child1",
                    Alias = "t2",
                    FieldMappings = new Dictionary<string, FieldParameterMapping>
                                        {
                                            {DataGenerator.GenerateString(), ObjectCreator.CreateNew<FieldParameterMapping>()}
                                        }
                });

            fieldHelper
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(), It.IsAny<Child2>(), It.IsAny<bool>(),
                    It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = "dbo.Child2",
                    Alias = "t3",
                    FieldMappings = new Dictionary<string, FieldParameterMapping>
                                        {
                                            {DataGenerator.GenerateString(), ObjectCreator.CreateNew<FieldParameterMapping>()}
                                        }
                });
            dynamic parameters = new ExpandoObject();
            Expression<Func<ParentClass, bool>> predicate = p => p.IsNotNull();

            parameters.Predicate = predicate;
            parameters.CanDirtyRead = true;
            parameters.IncludeParameters = true;
            parameters.DesiredFields = null;
            parameters.TableName = null;

            SystemUnderTest.BuildQuery<ParentClass>(parameters);

            Mocks.Get<IQueryBuilderStrategyFactory>()
                .Verify(x => x.GetBuilderStrategy(QueryKind.SelectSingleTable), Times.Never);
        }

        [Test]
        public void WillInvokeFieldHelper()
        {
            Mock<IFieldHelper> fieldHelper = null;


            fieldHelper = Mocks.Get<IFieldHelper>();

            fieldHelper
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(), 
                    It.IsAny<string>(), It.IsAny<ParentClass>(), It.IsAny<bool>(), 
                    It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = "dbo.Parent",
                    Alias = "t1",
                    FieldMappings =  new Dictionary<string, FieldParameterMapping>
                        {
                            {DataGenerator.GenerateString(), ObjectCreator.CreateNew<FieldParameterMapping>()}
                        }
                });

            fieldHelper
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(), It.IsAny<Child1>(), It.IsAny<bool>(), 
                    It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = "dbo.Child1",
                    Alias = "t2",
                    FieldMappings =  new Dictionary<string, FieldParameterMapping>
                        {
                            {DataGenerator.GenerateString(), ObjectCreator.CreateNew<FieldParameterMapping>()}
                        }
                });

            fieldHelper
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(), It.IsAny<Child2>(), It.IsAny<bool>(), 
                    It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = "dbo.Child2",
                    Alias = "t3",
                    FieldMappings = new Dictionary<string, FieldParameterMapping>
                        {
                            {DataGenerator.GenerateString(), ObjectCreator.CreateNew<FieldParameterMapping>()}
                        }
                });
            dynamic parameters = new ExpandoObject();
            Expression<Func<ParentClass, bool>> predicate = p => p.IsNotNull();
                
            parameters.Predicate = predicate;
            parameters.CanDirtyRead = true;
            parameters.IncludeParameters = true;
            parameters.DesiredFields = null;
            parameters.TableName = null;
            
            SystemUnderTest.BuildQuery<ParentClass>(parameters);

            fieldHelper
                .Verify(x => x.BuildFields(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), 
                    It.IsAny<ParentClass>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            fieldHelper
                .Verify(x => x.BuildFields(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(),
                    It.IsAny<Child1>(), It.IsAny<bool>(), 
                    It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            fieldHelper
                .Verify(x => x.BuildFields(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(),
                    It.IsAny<Child2>(), It.IsAny<bool>(), It.IsAny<string>(), 
                    It.IsAny<string>()), Times.Once); 
        }

        [Test]
        public void WillBuildSimpleJoinQuery()
        {
            QueryInfo expected = null;

            const string expectedQuery = "SELECT t1.ParentProperty1 AS t1_ParentProperty1, t2.ChildProperty1 AS t2_ChildProperty1, t2.ChildProperty2 AS t2_ChildProperty2, t2.ChildProperty3 AS t2_ChildProperty3, t3.ChildProperty1 AS t3_ChildProperty1, t3.ChildProperty2 AS t3_ChildProperty2, t3.ChildProperty3 AS t3_ChildProperty3 FROM dbo.Parent (NOLOCK) t1 INNER JOIN dbo.Child1 (NOLOCK) t2 ON t1.t1_ParentProperty1 = t2.t2_ChildProperty1 LEFT JOIN dbo.Child2 (NOLOCK) t3 ON t1.t1_ParentProperty1 = t3.t3_ChildProperty1;";

            var fieldHelper = Mocks.Get<IFieldHelper>();

            fieldHelper
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(), It.IsAny<ParentClass>(), It.IsAny<bool>(), "t1", 
                    It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = "dbo.Parent",
                    FieldMappings = new Dictionary<string, FieldParameterMapping>
                    {
                        {
                            "ParentProperty1", new FieldParameterMapping
                            {
                                FieldName = "ParentProperty1", 
                                Prefix = "t1",
                                DbType = SqlDbType.Int
                            }
                        }
                    }, 
                    Alias = "t1"
                });

            fieldHelper
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(), It.IsAny<Child1>(), It.IsAny<bool>(), "t2", 
                    It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = "dbo.Child1",
                    FieldMappings = new Dictionary<string, FieldParameterMapping>
                    {
                        {
                            "ChildProperty1", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty1",
                                Prefix = "t2",
                                DbType = SqlDbType.Int
                            }
                        },
                        {
                            "ChildProperty2", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty2",
                                Prefix = "t2",
                                DbType = SqlDbType.NVarChar
                            }
                        },
                        {
                            "ChildProperty3", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty3",
                                Prefix = "t2",
                                DbType = SqlDbType.NVarChar
                            }
                        }
                    }, 
                    Alias = "t2"
                });

            fieldHelper
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(), It.IsAny<Child2>(), It.IsAny<bool>(), "t3", 
                    It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = "dbo.Child2",
                    FieldMappings = new Dictionary<string, FieldParameterMapping>
                    {
                        {
                            "ChildProperty1", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty1",
                                Prefix = "t3",
                                DbType = SqlDbType.Int
                            }
                        },
                        {
                            "ChildProperty2", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty2",
                                Prefix = "t3",
                                DbType = SqlDbType.NVarChar
                            }
                        },
                        {
                            "ChildProperty3", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty3",
                                Prefix = "t3",
                                DbType = SqlDbType.NVarChar
                            }
                        }
                    }, 
                    Alias = "t3"
                });

            expected = new QueryInfo(expectedQuery);
       
            dynamic parameters = new ExpandoObject();

            parameters.Predicate = null;
            parameters.CanDirtyRead = true;
            parameters.IncludeParameters = true;
            parameters.DesiredFields = null;
            parameters.TableName = null;

            QueryInfo actual = SystemUnderTest.BuildQuery<ParentClass>(parameters);   
         
            Asserter.AssertEquality(expected.Query, actual.Query);
        }

        [Test]
        public void WillBuildComplexJoinQuery()
        {
            QueryInfo expected = null;

            const string expectedQuery = @"SELECT t1.ParentProperty3 AS t1_ParentProperty3, t2.ChildProperty1 AS t2_ChildProperty1, t2.ChildProperty2 AS t2_ChildProperty2, t2.ChildProperty3 AS t2_ChildProperty3 FROM dbo.ParentClass3 (NOLOCK) t1 LEFT JOIN dbo.Parent3_Child1 (NOLOCK) ON t1.t1_ParentProperty3 = dbo.Parent3_Child1.ParentProperty3 LEFT JOIN dbo.Child3 (NOLOCK) t2 ON dbo.Parent3_Child1.ChildProperty1 = t2.t2_ChildProperty1;";

            var fieldHelper = Mocks.Get<IFieldHelper>();

            fieldHelper
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(), It.IsAny<ParentClass3>(), It.IsAny<bool>(), "t1", 
                    It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = "dbo.ParentClass3",
                    FieldMappings = new Dictionary<string, FieldParameterMapping>
                    {
                        {
                            "ParentProperty3", new FieldParameterMapping
                            {
                                FieldName = "ParentProperty3", 
                                Prefix = "t1",
                                DbType = SqlDbType.Int
                            }
                        }
                    },
                    Alias = "t1"
                });

            fieldHelper
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(), It.IsAny<Child3>(), It.IsAny<bool>(), "t2", 
                    It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = "dbo.Child3",
                    FieldMappings = new Dictionary<string, FieldParameterMapping>
                    {
                        {
                            "ChildProperty1", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty1",
                                Prefix = "t2",
                                DbType = SqlDbType.Int
                            }
                        },
                        {
                            "ChildProperty2", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty2",
                                Prefix = "t2",
                                DbType = SqlDbType.NVarChar
                            }
                        },
                        {
                            "ChildProperty3", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty3",
                                Prefix = "t2",
                                DbType = SqlDbType.NVarChar
                            }
                        }
                    },
                    Alias = "t2"
                });

            expected = new QueryInfo(expectedQuery);

            dynamic parameters = new ExpandoObject();

            parameters.Predicate = null;
            parameters.CanDirtyRead = true;
            parameters.IncludeParameters = true;
            parameters.DesiredFields = null;
            parameters.TableName = null;

            QueryInfo actual = SystemUnderTest.BuildQuery<ParentClass3>(parameters);

            Asserter.AssertEquality(expected.Query, actual.Query);
        }

        [Test]
        public void WillMixSimpleAndComplexJoins()
        {
            QueryInfo expected = null;

            const string expectedQuery = @"SELECT t1.ParentProperty1 AS t1_ParentProperty1, t2.ChildProperty1 AS t2_ChildProperty1, t2.ChildProperty2 AS t2_ChildProperty2, t2.ChildProperty3 AS t2_ChildProperty3, t3.ChildProperty1 AS t3_ChildProperty1, t3.ChildProperty2 AS t3_ChildProperty2, t3.ChildProperty3 AS t3_ChildProperty3, t4.ChildProperty1 AS t4_ChildProperty1, t4.ChildProperty2 AS t4_ChildProperty2, t4.ChildProperty3 AS t4_ChildProperty3 FROM dbo.Parent2 (NOLOCK) t1 INNER JOIN dbo.Child1 (NOLOCK) t2 ON t1.t1_ParentProperty1 = t2.t2_ChildProperty1 LEFT JOIN dbo.Child2 (NOLOCK) t3 ON t1.t1_ParentProperty1 = t3.t3_ChildProperty1 LEFT JOIN dbo.Parent2_Child1 (NOLOCK) ON t1.t1_ParentProperty1 = dbo.Parent2_Child1.ParentProperty1 LEFT JOIN dbo.Child3 (NOLOCK) t4 ON dbo.Parent2_Child1.ChildProperty1 = t4.t4_ChildProperty1;";

            var fieldHelper = Mocks.Get<IFieldHelper>();

            fieldHelper
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(), It.IsAny<ParentClass2>(), It.IsAny<bool>(), "t1", 
                    It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = "dbo.Parent2",
                    FieldMappings = new Dictionary<string, FieldParameterMapping>
                    {
                        {
                            "ParentProperty1", new FieldParameterMapping
                            {
                                FieldName = "ParentProperty1", 
                                Prefix = "t1",
                                DbType = SqlDbType.Int
                            }
                        }
                    },
                    Alias = "t1"
                });

            fieldHelper
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(), It.IsAny<Child1>(), It.IsAny<bool>(), "t2", 
                    It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = "dbo.Child1",
                    FieldMappings = new Dictionary<string, FieldParameterMapping>
                    {
                        {
                            "ChildProperty1", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty1",
                                Prefix = "t2",
                                DbType = SqlDbType.Int
                            }
                        },
                        {
                            "ChildProperty2", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty2",
                                Prefix = "t2",
                                DbType = SqlDbType.NVarChar
                            }
                        },
                        {
                            "ChildProperty3", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty3",
                                Prefix = "t2",
                                DbType = SqlDbType.NVarChar
                            }
                        }
                    },
                    Alias = "t2"
                });

            fieldHelper
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(), It.IsAny<Child2>(), It.IsAny<bool>(), "t3", 
                    It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = "dbo.Child2",
                    FieldMappings = new Dictionary<string, FieldParameterMapping>
                    {
                        {
                            "ChildProperty1", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty1",
                                Prefix = "t3",
                                DbType = SqlDbType.Int
                            }
                        },
                        {
                            "ChildProperty2", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty2",
                                Prefix = "t3",
                                DbType = SqlDbType.NVarChar
                            }
                        },
                        {
                            "ChildProperty3", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty3",
                                Prefix = "t3",
                                DbType = SqlDbType.NVarChar
                            }
                        }
                    },
                    Alias = "t3"
                });
            
            fieldHelper
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(), It.IsAny<Child3>(), It.IsAny<bool>(), "t4", 
                    It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = "dbo.Child3",
                    FieldMappings = new Dictionary<string, FieldParameterMapping>
                    {
                        {
                            "ChildProperty1", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty1",
                                Prefix = "t4",
                                DbType = SqlDbType.Int
                            }
                        },
                        {
                            "ChildProperty2", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty2",
                                Prefix = "t4",
                                DbType = SqlDbType.NVarChar
                            }
                        },
                        {
                            "ChildProperty3", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty3",
                                Prefix = "t4",
                                DbType = SqlDbType.NVarChar
                            }
                        }
                    },
                Alias = "t4"
            });

            expected = new QueryInfo(expectedQuery);
            dynamic parameters = new ExpandoObject();

            parameters.Predicate = null;
            parameters.CanDirtyRead = true;
            parameters.IncludeParameters = true;
            parameters.DesiredFields = null;
            parameters.TableName = null;

            QueryInfo actual = SystemUnderTest.BuildQuery<ParentClass2>(parameters);

            Asserter.AssertEquality(expected.Query, actual.Query);
        }

        [Test]
        public void WillInvokePredicateBuilderIfPredicateIsNotNull()
        {
            const string expectedQuery = @"SELECT t1.ParentProperty3 AS t1_ParentProperty3, t2.ChildProperty1 AS t2_ChildProperty1, t2.ChildProperty2 AS t2_ChildProperty2, t2.ChildProperty3 AS t2_ChildProperty3 FROM dbo.ParentClass3 (NOLOCK) t1 LEFT JOIN dbo.Parent3_Child1 (NOLOCK) ON t1.t1_ParentProperty3 = dbo.Parent3_Child1.ParentProperty3 LEFT JOIN dbo.Child3 (NOLOCK) t2 ON dbo.Parent3_Child1.ChildProperty1 = t2.t2_ChildProperty1;";

                var fieldHelper = Mocks.Get<IFieldHelper>();

                fieldHelper
                    .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(),
                        It.IsAny<string>(), It.IsAny<ParentClass3>(), It.IsAny<bool>(), "t1", 
                        It.IsAny<string>()))
                    .Returns(new TableObjectMapping
                    {
                        TableName = "dbo.ParentClass3",
                        FieldMappings = new Dictionary<string, FieldParameterMapping>
                        {
                            {
                                "ParentProperty3", new FieldParameterMapping
                                {
                                    FieldName = "ParentProperty3", 
                                    Prefix = "t1",
                                    DbType = SqlDbType.Int
                                }
                            }
                        },
                        Alias = "t1"
                    });

                fieldHelper
                    .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(),
                        It.IsAny<string>(), It.IsAny<Child3>(), It.IsAny<bool>(), "t2", 
                        It.IsAny<string>()))
                    .Returns(new TableObjectMapping
                    {
                        TableName = "dbo.Child3",
                        FieldMappings = new Dictionary<string, FieldParameterMapping>
                        {
                            {
                                "ChildProperty1", new FieldParameterMapping
                                {
                                    FieldName = "ChildProperty1",
                                    Prefix = "t2",
                                    DbType = SqlDbType.Int
                                }
                            },
                            {
                                "ChildProperty2", new FieldParameterMapping
                                {
                                    FieldName = "ChildProperty2",
                                    Prefix = "t2",
                                    DbType = SqlDbType.NVarChar
                                }
                            },
                            {
                                "ChildProperty3", new FieldParameterMapping
                                {
                                    FieldName = "ChildProperty3",
                                    Prefix = "t2",
                                    DbType = SqlDbType.NVarChar
                                }
                            }
                        },
                        Alias = "t2"
                    });

                var query = new QueryInfo(expectedQuery);
        
                dynamic parameters = new ExpandoObject();
// ReSharper disable once ConditionIsAlwaysTrueOrFalse
                Expression<Func<ParentClass3, bool>> predicate = t => t.ParentProperty3 != null;

                parameters.Predicate = predicate;
                parameters.CanDirtyRead = true;
                parameters.IncludeParameters = true;
                parameters.DesiredFields = null;
                parameters.TableName = null;

                SystemUnderTest.BuildQuery<ParentClass3>(parameters);

                Mocks.Get<IPredicateBuilder>()
                    .Verify(x => x.BuildContainer(It.IsAny<Expression>(), It.IsAny<Type>(), 
                        It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void WillNotInvokePredicateBuilderIfPredicateIsNull()
        {
            QueryInfo expected = null;

            const string expectedQuery = @"SELECT t1.ParentProperty1 AS t1_ParentProperty1, t2.ChildProperty1 AS t2_ChildProperty1, t2.ChildProperty2 AS t2_ChildProperty2, t2.ChildProperty3 AS t2_ChildProperty3, t3.ChildProperty1 AS t3_ChildProperty1, t3.ChildProperty2 AS t3_ChildProperty2, t3.ChildProperty3 AS t3_ChildProperty3, t4.ChildProperty1 AS t4_ChildProperty1, t4.ChildProperty2 AS t4_ChildProperty2, t4.ChildProperty3 AS t4_ChildProperty3 FROM dbo.Parent2 (NOLOCK) t1 INNER JOIN dbo.Child1 (NOLOCK) t2 ON t1.t1_ParentProperty1 = t2.t2_ChildProperty1 LEFT JOIN dbo.Child2 (NOLOCK) t3 ON t1.t1_ParentProperty1 = t3.t3_ChildProperty1 LEFT JOIN dbo.Parent2_Child1 (NOLOCK) ON t1.t1_ParentProperty1 = dbo.Parent2_Child1.ParentProperty1 LEFT JOIN dbo.Child3 (NOLOCK) t4 ON dbo.Parent2_Child1.ChildProperty1 = t4.t4_ChildProperty1;";

            var fieldHelper = Mocks.Get<IFieldHelper>();

            fieldHelper
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(), It.IsAny<ParentClass2>(), It.IsAny<bool>(), "t1", 
                    It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = "dbo.Parent2",
                    FieldMappings = new Dictionary<string, FieldParameterMapping>
                    {
                        {
                            "ParentProperty1", new FieldParameterMapping
                            {
                                FieldName = "ParentProperty1", 
                                Prefix = "t1",
                                DbType = SqlDbType.Int
                            }
                        }
                    },
                    Alias = "t1"
                });

            fieldHelper
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(), It.IsAny<Child1>(), It.IsAny<bool>(), "t2", 
                    It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = "dbo.Child1",
                    FieldMappings = new Dictionary<string, FieldParameterMapping>
                    {
                        {
                            "ChildProperty1", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty1",
                                Prefix = "t2",
                                DbType = SqlDbType.Int
                            }
                        },
                        {
                            "ChildProperty2", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty2",
                                Prefix = "t2",
                                DbType = SqlDbType.NVarChar
                            }
                        },
                        {
                            "ChildProperty3", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty3",
                                Prefix = "t2",
                                DbType = SqlDbType.NVarChar
                            }
                        }
                    },
                    Alias = "t2"
                });

            fieldHelper
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(), It.IsAny<Child2>(), It.IsAny<bool>(), "t3", 
                    It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = "dbo.Child2",
                    FieldMappings = new Dictionary<string, FieldParameterMapping>
                    {
                        {
                            "ChildProperty1", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty1",
                                Prefix = "t3",
                                DbType = SqlDbType.Int
                            }
                        },
                        {
                            "ChildProperty2", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty2",
                                Prefix = "t3",
                                DbType = SqlDbType.NVarChar
                            }
                        },
                        {
                            "ChildProperty3", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty3",
                                Prefix = "t3",
                                DbType = SqlDbType.NVarChar
                            }
                        }
                    },
                    Alias = "t3"
                });

            fieldHelper
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(), It.IsAny<Child3>(), It.IsAny<bool>(), "t4", 
                    It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = "dbo.Child3",
                    FieldMappings = new Dictionary<string, FieldParameterMapping>
                    {
                        {
                            "ChildProperty1", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty1",
                                Prefix = "t4",
                                DbType = SqlDbType.Int
                            }
                        },
                        {
                            "ChildProperty2", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty2",
                                Prefix = "t4",
                                DbType = SqlDbType.NVarChar
                            }
                        },
                        {
                            "ChildProperty3", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty3",
                                Prefix = "t4",
                                DbType = SqlDbType.NVarChar
                            }
                        }
                    },
                    Alias = "t4"
                });

            expected = new QueryInfo(expectedQuery);

            dynamic parameters = new ExpandoObject();

            parameters.Predicate = null;
            parameters.CanDirtyRead = true;
            parameters.IncludeParameters = true;
            parameters.DesiredFields = null;
            parameters.TableName = null;

            SystemUnderTest.BuildQuery<ParentClass2>(parameters);

            Mocks.Get<IPredicateBuilder>()
                .Verify(x => x.BuildContainer(It.IsAny<Expression>(), It.IsAny<Type>(), 
                    It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void WillInvokePredicateBuilderWithPrefixIfPredicateIsNotNull()
        {
            QueryInfo expected = null;

            const string expectedQuery = @"SELECT t1.ParentProperty3 AS t1_ParentProperty3, t2.ChildProperty1 AS t2_ChildProperty1, t2.ChildProperty2 AS t2_ChildProperty2, t2.ChildProperty3 AS t2_ChildProperty3 FROM dbo.ParentClass3 (NOLOCK) t1 LEFT JOIN dbo.Parent3_Child1 (NOLOCK) ON t1.t1_ParentProperty3 = dbo.Parent3_Child1.ParentProperty3 LEFT JOIN dbo.Child3 (NOLOCK) t2 ON dbo.Parent3_Child1.ChildProperty1 = t2.t2_ChildProperty1;";

            var fieldHelper = Mocks.Get<IFieldHelper>();

            fieldHelper
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(), It.IsAny<ParentClass3>(), It.IsAny<bool>(), "t1", 
                    It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = "dbo.ParentClass3",
                    FieldMappings = new Dictionary<string, FieldParameterMapping>
                    {
                        {
                            "ParentProperty3", new FieldParameterMapping
                            {
                                FieldName = "ParentProperty3", 
                                Prefix = "t1",
                                DbType = SqlDbType.Int
                            }
                        }
                    },
                    Alias = "t1"
                });

            fieldHelper
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(), It.IsAny<Child3>(), It.IsAny<bool>(), "t2", 
                    It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = "dbo.Child3",
                    FieldMappings = new Dictionary<string, FieldParameterMapping>
                    {
                        {
                            "ChildProperty1", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty1",
                                Prefix = "t2",
                                DbType = SqlDbType.Int
                            }
                        },
                        {
                            "ChildProperty2", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty2",
                                Prefix = "t2",
                                DbType = SqlDbType.NVarChar
                            }
                        },
                        {
                            "ChildProperty3", new FieldParameterMapping
                            {
                                FieldName = "ChildProperty3",
                                Prefix = "t2",
                                DbType = SqlDbType.NVarChar
                            }
                        }
                    },
                    Alias = "t2"
                });

            expected = new QueryInfo(expectedQuery);

            dynamic parameters = new ExpandoObject();
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            Expression<Func<ParentClass3, bool>> predicate = t => t.ParentProperty3 != null;

            parameters.Predicate = predicate;
            parameters.CanDirtyRead = true;
            parameters.IncludeParameters = true;
            parameters.DesiredFields = null;
            parameters.TableName = null;

            SystemUnderTest.BuildQuery<ParentClass3>(parameters);

            Mocks.Get<IPredicateBuilder>()
                .Verify(x => x.BuildContainer(It.IsAny<Expression>(), It.IsAny<Type>(),
                    It.IsAny<bool>(), "t1", "t1_"), Times.Once);
        }

        [Table("dbo.NoJoins")]
        public class NoJoinAttributeClass
        {
            [FieldMetadata("Id", SqlDbType.Int)]
            public int Id { get; set; }
        }

        [Table("dbo.Parent")]
        public class ParentClass
        {
            [FieldMetadata("ParentProperty1", SqlDbType.Int)]
            public int ParentProperty1 { get; set; }

            [Join(JoinType.Inner, typeof(Child1), "ParentProperty1", "ChildProperty1")]
            public Child1 Child1 { get; set; }

            [Join(JoinType.Left, typeof(Child2), "ParentProperty1", "ChildProperty1")]
            public Child2 Child2 { get; set; }
        }
        
        [Table("dbo.Parent2")]
        public class ParentClass2
        {
            [FieldMetadata("ParentProperty1", SqlDbType.Int)]
            public int ParentProperty1 { get; set; }

            [Join(JoinType.Inner, typeof(Child1), "ParentProperty1", "ChildProperty1")]
            public Child1 Child1 { get; set; }

            [Join(JoinType.Left, typeof(Child2), "ParentProperty1", "ChildProperty1")]
            public Child2 Child2 { get; set; }

            [Join(JoinType.Left, typeof(Child3), "ParentProperty1", "ChildProperty1",
               "dbo.Parent2_Child1", "ParentProperty1", "ChildProperty1", JoinType.Left)]
            public Child3 Child3 { get; set; }
        }

        [Table("dbo.ParentClass3")]
        public class ParentClass3
        {
            [FieldMetadata("ParentProperty3", SqlDbType.Int)]
            public int ParentProperty3 { get; set; }

            [Join(JoinType.Left, typeof(Child3), "ParentProperty3", "ChildProperty1", 
                "dbo.Parent3_Child1", "ParentProperty3", "ChildProperty1", JoinType.Left)]
            public Child3 Child3 { get; set; }
        }

        [Table("dbo.Child1")]
        public class Child1
        {
            [FieldMetadata("ChildProperty1", SqlDbType.Int)]
            public int ChildProperty1 { get; set; }

            [FieldMetadata("ChildProperty2", SqlDbType.NVarChar)]
            public string ChildProperty2 { get; set; }

            [FieldMetadata("ChildProperty3", SqlDbType.NVarChar)]
            public string ChildProperty3 { get; set; }
        }

        [Table("dbo.Child2")]
        public class Child2
        {
            [FieldMetadata("ChildProperty1", SqlDbType.Int)]
            public int ChildProperty1 { get; set; }

            [FieldMetadata("ChildProperty2", SqlDbType.NVarChar)]
            public string ChildProperty2 { get; set; }

            [FieldMetadata("ChildProperty3", SqlDbType.NVarChar)]
            public string ChildProperty3 { get; set; }
        }

        [Table("dbo.Child3")]
        public class Child3
        {
            [FieldMetadata("ChildProperty1", SqlDbType.Int)]
            public int ChildProperty1 { get; set; }

            [FieldMetadata("ChildProperty2", SqlDbType.Int)]
            public string ChildProperty2 { get; set; }

            [FieldMetadata("ChildProperty3", SqlDbType.NVarChar)]
            public string ChildProperty3 { get; set; }
        }
    }
}
