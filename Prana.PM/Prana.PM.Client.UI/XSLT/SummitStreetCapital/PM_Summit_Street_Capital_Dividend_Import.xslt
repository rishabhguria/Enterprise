<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  xmlns:my1="put-your-namespace-uri-here"
  xmlns:my2="put-your-namespace-uri-here">

  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my1">
    <msxsl:assembly name="System.Data"/>
    <msxsl:using namespace="System.Data"/>
    <msxsl:using namespace="System.Globalization"/>

    public string NextBusinessDate(int year, int month, int date)
    {
    DateTime weekEnd = new DateTime(year, month, date);
    weekEnd = weekEnd.AddDays(-1);
    while (weekEnd.DayOfWeek == DayOfWeek.Saturday || weekEnd.DayOfWeek == DayOfWeek.Sunday || CheckHoliday(weekEnd))
    {
    weekEnd = weekEnd.AddDays(-1);
    }
    return weekEnd.ToString();
    }
	
    public bool CheckHoliday(DateTime date)
    {
    bool isHoliday = false;
    DataSet ds = new DataSet();

    //Change the xml path
    ds.ReadXml(@"C:\Nirvana\SummitStreet\Client Release\MappingFiles\ReconMappingXml/HolidayMapping.xml");

    for (int i = 0; i &lt; ds.Tables[1].Rows.Count; i++)
    {
    var tempp = ds.Tables[1].Rows[i][0].ToString();
    DateTime dt = DateTime.ParseExact(tempp, "MM/dd/yyyy", CultureInfo.InvariantCulture);
    if (DateTime.Compare(date, dt) == 0)
    {
    isHoliday = true;
    break;
    }
    }
    return isHoliday;
    }
  </msxsl:script>

  <msxsl:script language="C#" implements-prefix="my2">
    public static string GetTimeDate()
    {
    string currentDate = System.DateTime.Now.ToString();
    return currentDate;
    }
  </msxsl:script>
	
    <xsl:template name="Translate">
      <xsl:param name="Number" />
      <xsl:variable name="SingleQuote">'</xsl:variable>
      <xsl:variable name="varNumber">
        <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))" />
      </xsl:variable>
      <xsl:choose>
        <xsl:when test="contains($Number,'(')">
          <xsl:value-of select="$varNumber*-1" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$varNumber" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:template>

	<xsl:template match="/">
		
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="varAmount">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL12)"/>
					</xsl:call-template>
				</xsl:variable>		

				<xsl:if test="number($varAmount)">

					<PositionMaster>

						<xsl:variable name="PB_Name">
							<xsl:value-of select="'JEFF'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL16)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name = $PB_Name]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME" select="normalize-space(COL11)"/>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_Name]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<xsl:variable name="varSymbol" select="normalize-space(COL1)"/>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varSymbol !=''">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
						
						<Amount>
							<xsl:choose>
								<xsl:when test="number($varAmount)">
									<xsl:value-of select="$varAmount"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Amount>				

						<xsl:variable name="varPayoutDate">
							<xsl:value-of select="normalize-space(COL2)"/>
						</xsl:variable>

						<PayoutDate>
							<xsl:value-of select="$varPayoutDate"/>
						</PayoutDate>											
                      
                       <xsl:variable name="varPBFiledateCheckDate">
                         <xsl:value-of select="substring-before(substring-after(normalize-space(COL38),'REC'),'PAY')"/>
                       </xsl:variable>
						
					   <xsl:variable name="varCheckDate">
                         <xsl:value-of select="substring-before(substring-after(normalize-space($varPBFiledateCheckDate),'/'),'/')"/>
                       </xsl:variable>
						
					   <xsl:variable name="varCheckMonth">
                         <xsl:value-of select="substring-before(normalize-space($varPBFiledateCheckDate),'/')"/>
                       </xsl:variable>
						
					   <xsl:variable name="varCheckYear">
                         <xsl:value-of select="concat('20',substring-after(substring-after(normalize-space($varPBFiledateCheckDate),'/'),'/'))"/>
                       </xsl:variable>
						
						<xsl:variable name="CheckDate">
                           <xsl:value-of select="my1:NextBusinessDate(
                                                      number($varCheckYear),
                                                      number($varCheckMonth),
                                                      number($varCheckDate)
                                                    )"/>
                         </xsl:variable>
						
					    <!--<xsl:variable name="CheckDate">
                          <xsl:value-of select="my1:NextBusinessDate($varCheckMonth,$varCheckDate,$varCheckYear)"/>
                        </xsl:variable>-->			
																					
						<ExDate>
							<xsl:value-of select="$CheckDate"/>
						</ExDate>
						
						<!-- <PBSymbol> -->
							<!-- <xsl:value-of select="concat($varCheckMonth,'/',$varCheckDate,'/',$varCheckYear)"/> -->
						<!-- </PBSymbol> -->

						<xsl:variable name="varCurrency">							
							 <xsl:value-of select="COL23"/>							
						</xsl:variable>

						<CurrencyName>
							<xsl:value-of select="$varCurrency"/>
						</CurrencyName>

						<Description>
							<xsl:choose>
								<xsl:when test="normalize-space(COL6) &gt; 0">
									<xsl:value-of select="'Dividend Received'"/>
								</xsl:when>
								<xsl:when test ="normalize-space(COL6) &lt; 0">
									<xsl:value-of select ="'WithholdingTax'"/>
								</xsl:when>								
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Description>

						<ActivityType>
							<xsl:choose>
								<xsl:when test="normalize-space(COL12) &gt; 0">
									<xsl:value-of select="'DividendIncome'"/>
								</xsl:when>
								<xsl:when test ="normalize-space(COL12) &lt; 0">
									<xsl:value-of select ="'WithholdingTax'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</ActivityType>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>