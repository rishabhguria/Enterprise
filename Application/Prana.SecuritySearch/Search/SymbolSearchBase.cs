using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Prana.BusinessObjects.TextSearch.SymbolSearch;
using Prana.LogManager;
using Prana.SecuritySearch.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Version = Lucene.Net.Util.Version;

namespace Prana.SecuritySearch.Search
{
    class SymbolSearchBase : CommonSearchBase
    {
        /// <summary>
        /// This method is used for symbol search
        /// This method specified the input string, limit for the result count and fieldName on which search need to be perform.
        /// Then it creates a search query for and called the internal search method
        /// </summary>
        /// <param name="input"></param>
        /// <param name="limit"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public IEnumerable<SymbolSearchDataModel> symbolSearch(string input, int limit, string fieldName)
        {
            if (string.IsNullOrEmpty(input)) return new List<SymbolSearchDataModel>();

            //var terms = input.Trim().Replace("-", " ").Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
            //var terms = input.Trim().Select(x => x + "*");
            var searchQuery = input.Trim() + "*";
            return singleCriteriaSearch(searchQuery, limit, fieldName);
            //return _testSearch(searchQuery, input, fieldName);
        }

        /// <summary>
        /// This method is used to search the given query from the specified cache directory.
        /// It creates WildcardQuery object to search.
        /// This method is used to create only one criteria query
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="input"></param>
        /// <param name="limit"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public IEnumerable<SymbolSearchDataModel> singleCriteriaSearch(string searchQuery, int limit, string fieldName)
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            IEnumerable<SymbolSearchDataModel> results = null;
            var searcher = new IndexSearcher(_directory, false);

            try
            {
                if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) return new List<SymbolSearchDataModel>();

                var hits_limit = limit;

                //var parser = new MultiFieldQueryParser(Version.LUCENE_30, new[] { "TickerSymbol", "BloombergSymbol" }, analyzer);
                //var parser = new MultiFieldQueryParser(Version.LUCENE_30, new[] { "TickerSymbol" }, analyzer);
                //var query = parseQuery(searchQuery, parser);
                //var query = new PrefixQuery(new Term("TickerSymbol", searchQuery));

                //var query = new WildcardQuery(new Term("TickerSymbol", searchQuery));
                //Query query = new WildcardQuery(new Term("BloombergSymbol", searchQuery));
                Query query = new WildcardQuery(new Term(fieldName, searchQuery));
                BooleanQuery booleanQuery = new BooleanQuery();
                booleanQuery.Add(query, Occur.MUST);
                //var query = new RegexQuery(new Term("TickerSymbol", searchQuery.ToString()));
                //RegexQuery scriptQuery = new RegexQuery(new Term("TickerSymbol", searchQuery));
                //BooleanQuery query = new BooleanQuery();
                //query.Add(new WildcardQuery(new Term("TickerSymbol", searchQuery)),Occur.MUST);
                //var hits = searcher.TickerSymbolSearch(query, null, hits_limit, Sort.RELEVANCE).ScoreDocs;

                var hits = searcher.Search(booleanQuery, hits_limit).ScoreDocs;
                results = mapLuceneToDataList(hits, searcher);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                analyzer.Close();
                searcher.Dispose();
            }
            return results;
        }

        /// <summary>
        /// This method is used to search the given query from the specified cache directory.
        /// It creates WildcardQuery object to search.
        /// This method can be used to create multiple criteria query
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="input"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        //public IEnumerable<SymbolSearchDataModel> multipleCriteriaSearch(string searchQuery, string input, string fieldName)
        //{
        //    var analyzer = new StandardAnalyzer(Version.LUCENE_30);
        //    IEnumerable<SymbolSearchDataModel> results = null;
        //    var searcher = new IndexSearcher(_directory, false);

        //    try
        //    {
        //        if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) return new List<SymbolSearchDataModel>();

        //        var hits_limit = 1000;

        //        // search by single field
        //        if (!string.IsNullOrEmpty(fieldName))
        //        {
        //            var parser = new QueryParser(Version.LUCENE_30, fieldName, analyzer);
        //            var query = parseQuery(searchQuery, parser);
        //            var hits = searcher.Search(query, hits_limit).ScoreDocs;
        //            results = mapLuceneToDataList(hits, searcher);
        //        }
        //        // search by multiple fields (ordered by RELEVANCE)
        //        else
        //        {
        //            Query query4 = new WildcardQuery(new Term("TickerSymbol", "DL*"));
        //            Query query5 = new WildcardQuery(new Term("BloombergSymbol", "*T"));
        //            Query query6 = new WildcardQuery(new Term("BloombergSymbol", "BLOOMDL*"));

        //            BooleanQuery booleanQuery = new BooleanQuery();
        //            booleanQuery.Add(query4, Occur.MUST);
        //            booleanQuery.Add(query5, Occur.MUST);
        //            booleanQuery.Add(query6, Occur.MUST);
        //            var hits = searcher.Search(booleanQuery, hits_limit).ScoreDocs;
        //            results = mapLuceneToDataList(hits, searcher);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    finally
        //    {
        //        analyzer.Close();
        //        searcher.Dispose();
        //    }
        //    return results;
        //}

        private SymbolSearchDataModel mapLuceneDocumentToData(Document doc)
        {
            return new SymbolSearchDataModel
            {
                //Id = Convert.ToInt32(doc.Get("Id")),
                TickerSymbol = doc.Get("TickerSymbol"),
                BloombergSymbol = doc.Get("BloombergSymbol"),
                FactSetSymbol = doc.Get("FactSetSymbol"),
                ActivSymbol = doc.Get("ActivSymbol")
            };
        }

        private IEnumerable<SymbolSearchDataModel> mapLuceneToDataList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            return hits.Select(hit => mapLuceneDocumentToData(searcher.Doc(hit.Doc))).ToList();
        }

        //private Query parseQuery(string searchQuery, QueryParser parser)
        //{
        //    Query query;
        //    try
        //    {
        //        query = parser.Parse(searchQuery.Trim());
        //    }
        //    catch (ParseException)
        //    {
        //        query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
        //    }
        //    return query;
        //}


        /// <summary>
        /// This method is used to add List object into the cache directory
        /// This methos makes internal call to addToLuceneIndex method
        /// </summary>
        /// <param name="symbolDatas"></param>
        /// <param name="dynamicIndex"></param>
        public void addUpdateLuceneIndex(IEnumerable<SymbolSearchDataModel> symbolDatas, Boolean dynamicIndex)
        {
            if (dynamicIndex)
            {
                addToLuceneIndex(symbolDatas);
            }
            else
            {
                if (!File.Exists(Path.Combine(IndexingDir, "indexCreated")))
                {
                    addToLuceneIndex(symbolDatas, true);

                    var indexCreateFilePath = Path.Combine(IndexingDir, "indexCreated");

                    if (!File.Exists(indexCreateFilePath))
                        File.Create(indexCreateFilePath);
                }
            }
        }

        /// <summary>
        /// This method is used to add List object into the cache directory.
        /// This method creates an analyzer and writer to write to the cache directory.
        /// </summary>
        /// <param name="symbolDatas"></param>
        public void addToLuceneIndex(IEnumerable<SymbolSearchDataModel> symbolDatas, bool recreateIndex = false)
        {
            //var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            var analyzer = new SimpleAnalyzer();
            var writer = new IndexWriter(_directory, analyzer, recreateIndex, IndexWriter.MaxFieldLength.UNLIMITED);

            try
            {
                // add data to lucene search index (replaces older entry if any)

                foreach (var symbolData in symbolDatas)
                    addToLuceneIndex(symbolData, writer);

                // close handles
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                analyzer.Close();
                writer.Optimize();
                writer.Dispose();
            }
        }

        /// <summary>
        /// This is the actual method which writes the object to the cache directory and creates index for it.
        /// </summary>
        /// <param name="symbolData"></param>
        /// <param name="writer"></param>
        /// <param name="analyzer"></param>
        private void addToLuceneIndex(SymbolSearchDataModel symbolData, IndexWriter writer)//, Analyzer analyzer)
        {
            if (singleCriteriaSearch(symbolData.TickerSymbol, 1, "TickerSymbol").Count() == 0)
            {
                //var searchQuery = new TermQuery(new Term("TickerSymbol", symbolData.TickerSymbol.ToString()));
                //writer.DeleteDocuments(searchQuery);

                var doc = new Document();
                //doc.Add(new Field("TickerSymbol", symbolData.TickerSymbol.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("TickerSymbol", symbolData.TickerSymbol, Field.Store.YES, Field.Index.NOT_ANALYZED));
                //doc.Add(new Field("BloombergSymbol", symbolData.BloombergSymbol, Field.Store.YES, Field.Index.NO));
                doc.Add(new Field("BloombergSymbol", symbolData.BloombergSymbol, Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("FactSetSymbol", symbolData.FactSetSymbol, Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("ActivSymbol", symbolData.ActivSymbol, Field.Store.YES, Field.Index.NOT_ANALYZED));

                //doc.Add(new Field( ("TickerSymbol", Field.Store.YES, Field.Index.UN_TOKENIZED));

                writer.AddDocument(doc);
                //writer.UpdateDocument(new Term("TickerSymbol"), doc);
            }
        }


        public void addUpdateLuceneIndex(SymbolSearchDataModel symbolData, Boolean dynamicIndex)
        {
            addUpdateLuceneIndex(new List<SymbolSearchDataModel> { symbolData }, dynamicIndex);
        }
    }
}
