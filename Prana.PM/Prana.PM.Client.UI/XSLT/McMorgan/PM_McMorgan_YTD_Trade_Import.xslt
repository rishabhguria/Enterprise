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
    weekEnd = weekEnd.AddDays(-2);
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
    ds.ReadXml(@"C:\Users\Mohit.Sahu\Documents\Deepa Mam Jira 4550\7861/HolidayMapping.xml");

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
	

	<xsl:template name="FormatDate">
		<xsl:param name="DateTime"/>
		<!--  converts date time double number to 18/12/2009  -->
		<xsl:variable name="l">
			<xsl:value-of select="$DateTime + 68569 + 2415019"/>
		</xsl:variable>
		<xsl:variable name="n">
			<xsl:value-of select="floor(((4 * $l) div 146097))"/>
		</xsl:variable>
		<xsl:variable name="ll">
			<xsl:value-of select="$l - floor(((146097 * $n + 3) div 4))"/>
		</xsl:variable>
		<xsl:variable name="i">
			<xsl:value-of select="floor(((4000 * ($ll + 1)) div 1461001))"/>
		</xsl:variable>
		<xsl:variable name="lll">
			<xsl:value-of select="$ll - floor(((1461 * $i) div 4)) + 31"/>
		</xsl:variable>
		<xsl:variable name="j">
			<xsl:value-of select="floor(((80 * $lll) div 2447))"/>
		</xsl:variable>
		<xsl:variable name="nDay">
			<xsl:value-of select="$lll - floor(((2447 * $j) div 80))"/>
		</xsl:variable>
		<xsl:variable name="llll">
			<xsl:value-of select="floor(($j div 11))"/>
		</xsl:variable>
		<xsl:variable name="nMonth">
			<xsl:value-of select="floor($j + 2 - (12 * $llll))"/>
		</xsl:variable>
		<xsl:variable name="nYear">
			<xsl:value-of select="floor(100 * ($n - 49) + $i + $llll)"/>
		</xsl:variable>
		<xsl:variable name="varMonthUpdated">
			<xsl:choose>
				<xsl:when test="string-length($nMonth) = 1">
					<xsl:value-of select="concat('0',$nMonth)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$nMonth"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="nDayUpdated">
			<xsl:choose>
				<xsl:when test="string-length($nDay) = 1">
					<xsl:value-of select="concat('0',$nDay)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$nDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:value-of select="$varMonthUpdated"/>
		<xsl:value-of select="'/'"/>
		<xsl:value-of select="$nDayUpdated"/>
		<xsl:value-of select="'/'"/>
		<xsl:value-of select="$nYear"/>
	</xsl:template>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>
		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="/">
		
		<DocumentElement>
			
			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL4)"/>
					</xsl:call-template>
				</xsl:variable>
				
				<xsl:if test="number($Position)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name = "PB_COMPANY_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>								
										
						<xsl:variable name="varCUSIP">
							<xsl:value-of select="normalize-space(COL1)"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varCUSIP !=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_COMPANY_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
						
						<CUSIP>
							<xsl:choose>
								<xsl:when test="$varCUSIP !=''">
									<xsl:value-of select="$varCUSIP"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</CUSIP>
						
						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL2)"/>
										
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund" />
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_FUND_NAME" />
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>		
						
						<SideTagValue>
                          <xsl:choose>                          			                
			                <xsl:when test="normalize-space(COL9) = 'PURCHASE'">
			              	  <xsl:value-of select="'1'"/>
			                </xsl:when>
			                <xsl:when test="normalize-space(COL9) = 'SALE'">
			              	  <xsl:value-of select="'2'"/>
			                </xsl:when>							
                           </xsl:choose>
                         </SideTagValue> 

						<xsl:variable name="CostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL12)"/>
							</xsl:call-template>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>
								<xsl:when test="$CostBasis &gt; 0">
									<xsl:value-of select="$CostBasis"/>
								</xsl:when>
								<xsl:when test="$CostBasis &lt; 0">
									<xsl:value-of select="$CostBasis * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL10)"/>
							</xsl:call-template>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test="$Commission &gt; 0">
									<xsl:value-of select="$Commission"/>
								</xsl:when>
								<xsl:when test="$Commission &lt; 0">
									<xsl:value-of select="$Commission * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

		            	<xsl:variable name ="varSecFee">
		            	   <xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL11)"/>
							</xsl:call-template>
		            	</xsl:variable>
		            		            
		            	<SecFee>
		            	   <xsl:choose>
		            	     <xsl:when test ="$varSecFee &gt; 0">
		            	      <xsl:value-of select ="$varSecFee"/>
		            	     </xsl:when>
		            	     <xsl:when test ="$varSecFee &lt; 0">
		            	      <xsl:value-of select ="$varSecFee * -1"/>
		            	     </xsl:when>
		            	     <xsl:otherwise>
		            	      <xsl:value-of select ="0"/>
		            	     </xsl:otherwise>
		            	   </xsl:choose>
		            	</SecFee>
						
						<xsl:variable name ="varFees">
		            	   <xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL13)"/>
							</xsl:call-template>
		            	</xsl:variable>
						
			            <Fees>			          	
			          	   <xsl:choose>
			          	    <xsl:when test ="$varFees &gt; 0">
			          	     <xsl:value-of select ="$varFees"/>
			          	    </xsl:when>
			          	    <xsl:when test ="$varFees &lt; 0">
			          	     <xsl:value-of select ="$varFees * -1"/>
			          	    </xsl:when>
			          	    <xsl:otherwise>
			          	     <xsl:value-of select ="0"/>
			          	    </xsl:otherwise>
			          	   </xsl:choose>
			            </Fees>

						<NetPosition>
							<xsl:choose>
								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="$Position"/>
								</xsl:when>
								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="$Position * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>							
			            					
						<PositionStartDate>
							<xsl:value-of select="normalize-space(COL5)"/>
						</PositionStartDate>
						
						<!--<xsl:variable name="varCheckDate">
                           <xsl:value-of select="substring-before(substring-after(normalize-space(COL5),'/'),'/')"/>
                        </xsl:variable>
						
					   <xsl:variable name="varCheckMonth">
                         <xsl:value-of select="substring-before(normalize-space(COL5),'/')"/>
                       </xsl:variable>
					   					
					   <xsl:variable name="varCheckYear">
                         <xsl:value-of select="substring-after(substring-after(normalize-space(COL5),'/'),'/')"/>
                       </xsl:variable>
						
						<xsl:variable name="CheckDate">
                           <xsl:value-of select="my1:NextBusinessDate(
                                                      number($varCheckYear),
                                                      number($varCheckMonth),
                                                      number($varCheckDate)
                                                    )"/>
                         </xsl:variable>-->

					    <!--<xsl:variable name="CheckDate">
                          <xsl:value-of select="my1:NextBusinessDate($varCheckMonth,$varCheckDate,$varCheckYear)"/>
                        </xsl:variable>-->

						<!--<PositionSettlementDate>
							<xsl:value-of select="$CheckDate"/>
						</PositionSettlementDate>-->

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


