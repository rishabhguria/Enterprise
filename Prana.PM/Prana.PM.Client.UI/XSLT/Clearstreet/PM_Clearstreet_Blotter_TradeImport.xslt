<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<!--Third Friday check-->
	<msxsl:script language="C#" implements-prefix="my">
		public string Now(int year, int month)
		{
		DateTime thirdFriday= new DateTime(year, month, 15);
		while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
		{
		thirdFriday = thirdFriday.AddDays(1);
		}
		return thirdFriday.ToString();
		}
	</msxsl:script>
	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month='JAN'">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month='FEB'">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month='MAR'">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month='APR'">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month='MAY'">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month='JUN'">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month='JUL'">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month='AUG'">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month='SEP'">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month='OCT'">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month='NOV'">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month='DEC'">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month='JAN'">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month='FEB'">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month='MAR'">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month='APR'">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month='MAY'">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month='JUN'">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month='JUL'">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month='AUG'">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month='SEP'">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month='OCT'">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month='NOV'">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month='DEC'">
					<xsl:value-of select="'X'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>
	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="substring-before(COL18,' ')='C' or substring-before(COL18,' ')='P'">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="normalize-space(COL36)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring-before(substring-after(COL37,'/'),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring-before(substring-after(substring-after(COL18,' '),' '),' ')"/>
				<!--<xsl:choose>
					<xsl:when test="string-length(substring-before(normalize-space(COL37),'/'))= '1'">
						<xsl:value-of select="concat('0',substring-before(normalize-space(COL37),'/'))"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="substring-before(normalize-space(COL37),'/')"/>
					</xsl:otherwise>
				</xsl:choose>-->
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after(substring-after(COL37,'/'),'/'),3,2)"/>
			</xsl:variable>
			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring-before(COL18,' ')"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(COL38,'##.00')"/>
				<!--<xsl:value-of select =" substring-before(substring-after(substring-after(normalize-space(COL8),' '),' '),' ')"/>-->
				<!--
				<xsl:value-of select="format-number(substring(substring-after(substring-after(COL5,' '),' '),2),'##.00')"/>-->
			</xsl:variable>
			<xsl:variable name="MonthCodVar">
				<xsl:call-template name="MonthCode">
					<xsl:with-param name="Month" select="$ExpiryMonth"/>
					<xsl:with-param name="PutOrCall" select="$PutORCall"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="Day">
				<xsl:choose>
					<xsl:when test="substring($ExpiryDay,1,1)='0'">
						<xsl:value-of select="substring($ExpiryDay,2,1)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$ExpiryDay"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="ThirdFriday">
				<xsl:choose>
					<xsl:when test="number($ExpiryMonth) and number($ExpiryYear)">
						<xsl:value-of select="my:Now(number($ExpiryYear),number($ExpiryMonth))"/>
					</xsl:when>
				</xsl:choose>
			</xsl:variable>
			<!--<xsl:choose>
				<xsl:when test="number($ExpiryMonth)=1 and $ExpiryYear='15'">

				</xsl:when>

				<xsl:otherwise>-->
			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)"/>
			<!--</xsl:otherwise>-->
			<!--

			</xsl:choose>-->
		</xsl:if>
	</xsl:template>
	<xsl:template name="spaces">
		<xsl:param name="count"/>
		<xsl:if test="number($count)">
			<xsl:call-template name="spaces">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:value-of select="' '"/>
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
			<xsl:for-each select="//PositionMaster">
				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL21"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="number($Position) and normalize-space(COL18)='insert' and COL4!='100000' and COL4!='100001' and COL4!='100002' and COL4!='100003' and COL4!='100004' and COL4!='100006' and COL4!='100007' and COL4!='100008' and COL4!='100012' and COL4!='100013' and COL4!='100038' and COL4!='100057' and COL2!='Precision Securities LLC' and COL2!='CenterPoint Securities'">
					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'ClearStreet'"/>
						</xsl:variable>
						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="COL15='Common Equity'">
									<xsl:value-of select="'Equity'"/>
								</xsl:when>
								<xsl:when test="COL15='Composite Unit'">
									<xsl:value-of select="'Equity'"/>
								</xsl:when>
								<xsl:when test="COL15='Exchange Traded Fund'">
									<xsl:value-of select="'Equity'"/>
								</xsl:when>
								<xsl:when test="COL15='Mutual Fund/Unit Investment Trust'">
									<xsl:value-of select="'Equity'"/>
								</xsl:when>
								<xsl:when test="COL15='Preferred Equity'">
									<xsl:value-of select="'Equity'"/>
								</xsl:when>
								<xsl:when test="COL15='Right'">
									<xsl:value-of select="'PrivateEquity'"/>
								</xsl:when>
								<xsl:when test="COL15='Warrant'">
									<xsl:value-of select="'PrivateEquity'"/>
								</xsl:when>
								<xsl:when test="COL15='Government/Agency Bond'">
									<xsl:value-of select="'fixedIncome'"/>
								</xsl:when>
								<xsl:when test="COL15='Corporate Bond'">
									<xsl:value-of select="'fixedIncome'"/>
								</xsl:when>
								<xsl:when test="COL15='Barrier Option'">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="PB_COMPANY">
							<xsl:choose>
								<xsl:when test="$Asset='PrivateEquity'">
									<xsl:value-of select="normalize-space(COL12)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="normalize-space(COL10)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
						</xsl:variable>
						<xsl:variable name="PB_FUND_NAME">
							<xsl:value-of select="concat(COL6,' ',COL4)"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>
						<CounterPartyID>
							<xsl:value-of select="'1'"/>
						</CounterPartyID>
						<xsl:variable name="Symbol">
							<xsl:value-of select="COL9"/>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL != ''">
									<xsl:value-of select="$PRANA_SYMBOL"/>
								</xsl:when>
								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_COMPANY"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
						<PBSymbol>
							<xsl:value-of select="$PB_COMPANY"/>
						</PBSymbol>
						<xsl:variable name="AvgPX">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL22"/>
							</xsl:call-template>
						</xsl:variable>
						<CostBasis>
							<xsl:choose>
								<xsl:when test="$AvgPX &gt; 0">
									<xsl:value-of select="$AvgPX"/>
								</xsl:when>
								<xsl:when test="$AvgPX &lt; 0">
									<xsl:value-of select="$AvgPX * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>
						<xsl:variable name="Date">
							<xsl:value-of select="concat(substring(COL19,5,2),'/',substring(COL19,7,2),'/',substring(COL19,1,4))"/>
						</xsl:variable>
						<PositionStartDate>
							<xsl:value-of select="$Date"/>
						</PositionStartDate>
						<xsl:variable name="Date1">
							<xsl:value-of select="concat(substring(COL20,5,2),'/',substring(COL20,7,2),'/',substring(COL20,1,4))"/>
						</xsl:variable>
						<PositionSettlementDate>
							<xsl:value-of select="$Date1"/>
						</PositionSettlementDate>
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
						<xsl:variable name="Side">
							<xsl:value-of select="COL16"/>
						</xsl:variable>
						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$Side = 'buy' and COL17='*'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$Side = 'buy' and COL17='short'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$Side = 'buy' and COL17='close' ">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:when test="$Side = 'sell' and COL17='short' ">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:when test="$Side = 'sell' and COL17='close' ">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="$Side = 'sell' and COL17='*'  ">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>
						
						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL25"/>
							</xsl:call-template>
						</xsl:variable>
						
						<!-- <xsl:variable name="ECN"> -->
							<!-- <xsl:value-of select="COL33"/> -->
						<!-- </xsl:variable> -->
						
						<!-- <xsl:variable name="ECN1"> -->
							<!-- <xsl:choose> -->
								<!-- <xsl:when test="$ECN &gt; 0"> -->
									<!-- <xsl:value-of select="$ECN"/> -->
								<!-- </xsl:when> -->
								<!-- <xsl:when test="$ECN &lt; 0"> -->
									<!-- <xsl:value-of select="$ECN * (1)"/> -->
								<!-- </xsl:when> -->
								<!-- <xsl:otherwise> -->
									<!-- <xsl:value-of select="'0'"/> -->
								<!-- </xsl:otherwise> -->
							<!-- </xsl:choose> -->
						<!-- </xsl:variable> -->
						
						<!-- <xsl:variable name="Commissionnew"> -->
							<!-- <xsl:value-of select="$Commission - $ECN1"/> -->
						<!-- </xsl:variable> -->
						
						<Commission>
							<xsl:choose>
								<xsl:when test="$Commission &gt; 0">
									<xsl:value-of select="$Commission"/>
								</xsl:when>
								<xsl:when test="$Commission &lt; 0">
									<xsl:value-of select="$Commission * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'0'"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>
						
						<!-- <OccFee> -->
							<!-- <xsl:choose> -->
								<!-- <xsl:when test="$ECN1 &gt; 0"> -->
									<!-- <xsl:value-of select="$ECN1"/> -->
								<!-- </xsl:when> -->
								<!-- <xsl:when test="$ECN1 &lt; 0"> -->
									<!-- <xsl:value-of select="$ECN1 * (1)"/> -->
								<!-- </xsl:when> -->
								<!-- <xsl:otherwise> -->
									<!-- <xsl:value-of select="0"/> -->
								<!-- </xsl:otherwise> -->
							<!-- </xsl:choose> -->
						<!-- </OccFee> -->
						
						<xsl:variable name="OrfFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL37"/>
							</xsl:call-template>
						</xsl:variable>
						<OrfFee>
							<xsl:choose>
								<xsl:when test="number($OrfFee)">
									<xsl:value-of select="$OrfFee"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</OrfFee>
						
						<xsl:variable name="SecFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL29"/>
							</xsl:call-template>
						</xsl:variable>
						<SecFee>
							<xsl:choose>
								<xsl:when test="$SecFee &gt; 0">
									<xsl:value-of select="$SecFee"/>
								</xsl:when>
								<xsl:when test="$SecFee &lt; 0">
									<xsl:value-of select="$SecFee * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SecFee>
						
						<!-- <xsl:variable name="Fees2"> -->
							<!-- <xsl:choose> -->
								<!-- <xsl:when test="COL16='buy'"> -->
									<!-- <xsl:value-of select="(COL31 - ((COL22 * COL21) + COL24 + COL25 + COL26 + COL27 + COL28 + COL29))"/> -->
								<!-- </xsl:when> -->
								<!-- <xsl:otherwise> -->
									<!-- <xsl:value-of select="(((COL22 * COL21) - COL24 - COL25 - COL26 - COL27 - COL28 - COL29) - COL31)"/> -->
								<!-- </xsl:otherwise> -->
							<!-- </xsl:choose> -->
						<!-- </xsl:variable> -->
						
						<xsl:variable name="Fees2">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL30"/>
							</xsl:call-template>
						</xsl:variable>
						<Fees>
							<xsl:choose>
								<xsl:when test="$Fees2 &gt; 0">
									<xsl:value-of select="1 * $Fees2"/>
								</xsl:when>
								<xsl:when test="$Fees2 &lt; 0">
									<xsl:value-of select="1 * $Fees2"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>
						
						<xsl:variable name="AccruedInterest">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="''"/>
							</xsl:call-template>
						</xsl:variable>
						
						<AccruedInterest>
							<xsl:choose>
								<xsl:when test="$AccruedInterest &gt; 0">
									<xsl:value-of select="$AccruedInterest"/>
								</xsl:when>
								<xsl:when test="$AccruedInterest &lt; 0">
									<xsl:value-of select="$AccruedInterest * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccruedInterest>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<!-- variable declaration for lower to upper case -->
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>


