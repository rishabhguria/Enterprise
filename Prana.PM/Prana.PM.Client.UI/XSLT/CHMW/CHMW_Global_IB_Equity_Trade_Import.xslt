<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<!--<msxsl:script language="C#" implements-prefix="my">

		public string Now(int year, int month)
		{
		DateTime thirdFriday= new DateTime(year, month, 15);
		while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
		{
		thirdFriday = thirdFriday.AddDays(1);
		}
		return thirdFriday.ToString();
		}

	</msxsl:script>-->

	<msxsl:script language="C#" implements-prefix="my">

		public string Now(int year, int month, int date,int day)
		{
		DateTime weekEnd= new DateTime(year, month, date);
		weekEnd = weekEnd.AddDays(day);
		while (weekEnd.DayOfWeek == DayOfWeek.Saturday || weekEnd.DayOfWeek == DayOfWeek.Sunday)
		{
		weekEnd = weekEnd.AddDays(1);
		}
		return weekEnd.ToString();
		}

		public string AddDay(int year, int month, int date)
		{
		DateTime weekEnd = new DateTime(year, month, date);
		return weekEnd.AddDays(1).ToString();
		}


	</msxsl:script>

	<xsl:template name="MonthName">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month = 'Jan'">
				<xsl:value-of select="'01'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Feb'">
				<xsl:value-of select="'02'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Mar'">
				<xsl:value-of select="'03'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Apr'">
				<xsl:value-of select="'04'"/>
			</xsl:when>
			<xsl:when test="$Month = 'May'">
				<xsl:value-of select="'05'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Jun'">
				<xsl:value-of select="'06'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Jul'">
				<xsl:value-of select="'07'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Aug'">
				<xsl:value-of select="'08'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Sep'">
				<xsl:value-of select="'09'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Oct'">
				<xsl:value-of select="'10'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Nov'">
				<xsl:value-of select="'11'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Dec'">
				<xsl:value-of select="'12'"/>
			</xsl:when>
		</xsl:choose>
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


	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month=01 ">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month=02 ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month=03 ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month=04 ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month=05 ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month=06 ">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month=07  ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month=08  ">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month=09 ">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month=01 ">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month=02 ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month=03 ">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month=04 ">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month=05 ">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month=06 ">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month=07  ">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month=08  ">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month=09 ">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
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
		<xsl:if test="contains(COL7,'CALL') or contains(COL7,'PUT')">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before($Symbol,'1')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),5,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),3,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),1,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),7,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring(substring-after($Symbol,$UnderlyingSymbol),8) div 1000  ,'#.00')"/>
			</xsl:variable>


			<xsl:variable name="MonthCodeVar">
				<xsl:call-template name="MonthCode">
					<xsl:with-param name="Month" select="number($ExpiryMonth)"/>
					<xsl:with-param name="PutOrCall" select="$PutORCall"/>
				</xsl:call-template>
				<!--<xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),5,1)"/>-->
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
			<!--<xsl:variable name="ThirdFriday">
				<xsl:choose>
					<xsl:when test="number($ExpiryMonth) and number($ExpiryYear)">
						<xsl:value-of select="my:Now(number($ExpiryYear),number($ExpiryMonth))"/>
					</xsl:when>
				</xsl:choose>
			</xsl:variable>-->
			<!--<xsl:choose>
				<xsl:when test="number($ExpiryMonth)=1 and $ExpiryYear='15'">

					<xsl:choose>
						<xsl:when test="substring(substring-after($ThirdFriday,'/'),1,2) = ($ExpiryDay - 1)">
							<xsl:value-of select="concat('O:',$UnderlyingSymbol,$Suffix,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="concat('O:',$UnderlyingSymbol,$Suffix,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>

				<xsl:otherwise>-->
			<xsl:value-of select="concat('O:',$UnderlyingSymbol,'',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
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

	<!--<xsl:template name="Option">
    <xsl:param name="Symbol"/>
    <xsl:param name="Suffix"/>
    <xsl:if test="COL18='Option'">

      -->
	<!--</xsl:otherwise>-->
	<!--
      -->
	<!--

			</xsl:choose>-->
	<!--
    </xsl:if>-->
	<!--
  </xsl:template>-->

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">


				<xsl:variable name="NetPosition">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL29"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($NetPosition) and COL6!='CASH' ">

					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Interactive Brokers'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<!--<xsl:choose>
								<xsl:when test="COL6='FUT'">
									<xsl:value-of select="COL7"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL8"/>
								</xsl:otherwise>
							</xsl:choose>-->
							<xsl:value-of select="COL8"/>
						</xsl:variable>


						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../../../MappingFiles/ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="COL6='FUT'">
									<xsl:value-of select="'Future'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="Symbol">
							<xsl:choose>
								<xsl:when test="COL4='USD' and COL6='STK'">
									<xsl:value-of select="concat(COL7,' ','US EQUITY')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>

						<xsl:variable name="ISIN">
							<xsl:choose>
								<xsl:when test="COL13!='*' and COL4!='USD'">
									<xsl:value-of select="COL13"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$ISIN!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="COL6='FUT'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="COL6='FOP'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<Bloomberg>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>

								<xsl:when test="$ISIN!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="COL6='FUT'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="COL6='FOP'">
									<xsl:value-of select="''"/>
								</xsl:when>


								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</Bloomberg>


						<ISIN>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$ISIN!=''">
									<xsl:value-of select="$ISIN"/>
								</xsl:when>

								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="COL6='FUT'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="COL6='FOP'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</ISIN>



						<Symbology>
							<xsl:choose>
								
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="'Symbol'"/>
								</xsl:when>
								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="'Bloomberg'"/>
								</xsl:when>
								<xsl:when test="$ISIN!=''">
									<xsl:value-of select="'ISIN'"/>
								</xsl:when>
								<xsl:when test="COL6='FUT'">
									<xsl:value-of select="'Bloomberg'"/>
								</xsl:when>

								<xsl:when test="COL6='FOP'">
									<xsl:value-of select="'Bloomberg'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbology>

						<SecurityIdentifier>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="COL6='FUT'">
									<xsl:value-of select="COL8"/>
								</xsl:when>

								<xsl:when test="COL6='FOP'">
									<xsl:value-of select="COL8"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SecurityIdentifier>

						<xsl:variable name="PB_FUND_NAME" select="COL1"/>

						<xsl:variable name ="prana_fund_name">
							<xsl:value-of select ="document('../../../Mappingfiles/ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>



						<FundName>
							<xsl:choose>

								<xsl:when test ="$prana_fund_name!=''">
									<xsl:value-of select ="$prana_fund_name"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</FundName>


						<NetPosition>
							<xsl:choose>
								<xsl:when test="$NetPosition &gt; 0">
									<xsl:value-of select="$NetPosition"/>
								</xsl:when>
								<xsl:when test="$NetPosition &lt; 0">
									<xsl:value-of select="$NetPosition* (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<xsl:variable name="PB_Contract" select="COL8"/>

						<xsl:variable name="PRANA_Multiplier">
							<xsl:value-of select ="document('../../../MappingFiles/ReconMappingXml/MultiplierMapping.xml')/SymbolMultiplierMapping/PB[@Name=$PB_NAME]/MultiplierData[@Contract=$PB_Contract]/@PBMultiplier"/>
						</xsl:variable>

						<xsl:variable name="PRANA_MultiplierToExclude">
							<xsl:value-of select ="document('../../../MappingFiles/ReconMappingXml/MultiplierMappingToExclude.xml')/SymbolMultiplierMapping/PB[@Name=$PB_NAME]/MultiplierData[@Contract=$PB_Contract]/@PBMultiplier"/>
						</xsl:variable>

						<xsl:variable name="Price">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL30"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="CostBasis">

							<xsl:choose>
								<xsl:when test="$PRANA_MultiplierToExclude!=''">
									<xsl:value-of select="$Price"/>
								</xsl:when>
								<xsl:when test="$PRANA_Multiplier!=''">
									<xsl:value-of select="format-number($Price * $PRANA_Multiplier,'#.######')"/>
									<!--<xsl:value-of select="$Price * $PRANA_Multiplier"/>-->
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select='number($Price)'/>
								</xsl:otherwise>
							</xsl:choose>
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


						<!--<xsl:variable name="Side" select="normalize-space(COL3)"/>-->

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="COL48='BUY (Ca.)' and (COL37='*' or COL37='')">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="COL48='BUY (Ca.)' and COL37='O'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="COL48='BUY (Ca.)' and COL37='C'">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:when test="COL48='SELL (Ca.)' and (COL37='*' or COL37='')">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="COL48='SELL (Ca.)' and COL37='O'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:when test="COL48='SELL (Ca.)' and COL37='C'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="contains(COL37,'O') and COL48='BUY'">
									<xsl:value-of select="'1'"/>
								</xsl:when>

								<xsl:when test="contains(COL37,'C') and COL48='BUY'">
									<xsl:value-of select="'B'"/>
								</xsl:when>

								<xsl:when test="contains(COL37,'O') and COL48='SELL'">
									<xsl:value-of select="'5'"/>
								</xsl:when>

								<xsl:when test="contains(COL37,'C') and COL48='SELL'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>


							<!--<xsl:choose>
								<xsl:when test="contains(COL37,'O') and COL48='BUY'">
									<xsl:value-of select="'1'"/>
								</xsl:when>

								<xsl:when test="contains(COL37,'C') and COL48='BUY'">
									<xsl:value-of select="'B'"/>
								</xsl:when>

								<xsl:when test="contains(COL37,'O') and COL48='SELL'">
									<xsl:value-of select="'5'"/>
								</xsl:when>

								<xsl:when test="contains(COL37,'C') and COL48='SELL'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>-->
						</SideTagValue>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>

						<xsl:variable name ="Date" select="COL24"/>


						<xsl:variable name="Year1" select="substring($Date,1,4)"/>
						<xsl:variable name="Month" select="substring($Date,5,2)"/>
						<xsl:variable name="Day" select="substring($Date,7,2)"/>



						<PositionStartDate>
							<xsl:value-of select="concat($Month,'/',$Day,'/',$Year1)"/>
							<!--<xsl:value-of select="COL24"/>-->
						</PositionStartDate>

						<xsl:variable name ="Date1" select="COL26"/>

						<xsl:variable name="Year" select="substring($Date1,1,4)"/>
						<xsl:variable name="Month1" select="substring($Date1,5,2)"/>
						<xsl:variable name="Day1" select="substring($Date1,7,2)"/>

						<xsl:variable name="SettleDate">
							<xsl:value-of select='my:Now(number($Year1),number($Month),number($Day),2)'/>
						</xsl:variable>

						<xsl:variable name="Month3">
							<xsl:call-template name="MonthName">
								<xsl:with-param name="Month" select="substring-before(COL1,' ')"/>
							</xsl:call-template>
						</xsl:variable>


						<xsl:variable name="varYear">
							<xsl:value-of select="substring-after(substring-after(COL1,' '),' ')"/>
						</xsl:variable>

						<xsl:variable name="varMonth">
							<xsl:value-of select="$Month1"/>
						</xsl:variable>


						<xsl:variable name="varDate">
							<xsl:value-of select="substring-before(substring-after(COL1,' '),',')"/>
						</xsl:variable>

						<!--<xsl:variable name="SettleDate">
							<xsl:value-of select='my:Now(number($Year),number($Month1),number($Day1 + 1))'/>
						</xsl:variable>

						<xsl:variable name="Date">
							<xsl:value-of select="COL1"/>
						</xsl:variable>-->

						<xsl:variable name="PRANA_Date_Mapping">
							<xsl:value-of select="document('../../../MappingFiles/ReconMappingXML/HolidayMapping.xml')/DateMapping/PB[@Name=$PB_NAME]/DateData/@Date"/>
						</xsl:variable>

						<xsl:variable name="DateCheck">
							<xsl:value-of select ="concat($Month,'/',$Day1,'/',$Year)"/>
						</xsl:variable>

						<xsl:variable name="MonthMap">
							<xsl:value-of select="substring-before($PRANA_Date_Mapping,'/')"/>
						</xsl:variable>

						<xsl:variable name="DayMap">
							<xsl:choose>
								<xsl:when test="string-length(number(substring-before(substring-after($PRANA_Date_Mapping,'/'),'/'))) = 1">
									<xsl:value-of select="concat(0,substring-before(substring-after($PRANA_Date_Mapping,'/'),'/') + 1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-before(substring-after($PRANA_Date_Mapping,'/'),'/')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="YearMap">
							<xsl:value-of select="substring-after(substring-after($PRANA_Date_Mapping,'/'),'/')"/>
						</xsl:variable>

						<xsl:variable name="varSettleDate">
							<xsl:value-of select="substring-before(my:AddDay(number(substring-before(substring-after(substring-after($SettleDate,'/'),'/'),' ')),number(substring-before($SettleDate,'/')),number(substring-before(substring-after($SettleDate,'/'),'/'))),' ')"/>
						</xsl:variable>

						<PositionSettlementDate>
							<xsl:choose>
								<xsl:when test="contains(COL27,'Cancel')">
									<xsl:choose>
										<xsl:when test="$PRANA_Date_Mapping = $DateCheck">
											<xsl:value-of select="concat($MonthMap,'/',$DayMap,'/',$YearMap) "/>
										</xsl:when>
										<xsl:otherwise>
											<!--<xsl:value-of select="concat($Month3,'/',$Day,'/',$Year)"/>-->
											<!--<xsl:value-of select="concat(substring-before($SettleDate,'/'),'/',substring-before(substring-after($SettleDate,'/'),'/') + 1,'/',substring-before(substring-after(substring-after($SettleDate,'/'),'/'),' '))"/>-->
											<!--<xsl:value-of select="substring-before(my:AddDay(number(substring-before(substring-after(substring-after($SettleDate,'/'),'/'),' ')),number(substring-before($SettleDate,'/')),number(substring-before(substring-after($SettleDate,'/'),'/'))),' ')"/>-->
											<xsl:choose>
												<xsl:when test="string-length(substring-before(substring-after($varSettleDate,'/'),'/'))=1">
													<xsl:value-of select="concat(substring-before($varSettleDate,'/'),'/',0,substring-before(substring-after($varSettleDate,'/'),'/'),'/',substring-after(substring-after($varSettleDate,'/'),'/'))"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="$varSettleDate"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat($Month1,'/',$Day1,'/',$Year)"/>
								</xsl:otherwise>
							</xsl:choose>




							<!--<xsl:value-of select="concat($Month1,'/',$Day1,'/',$Year)"/>-->
						</PositionSettlementDate>

						<xsl:variable name ="varCommision">
							<xsl:value-of select ="COL34"/>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test="contains(COL27,'Cancel')">
									<xsl:choose>

										<xsl:when test ="$varCommision &lt;0">
											<xsl:value-of select ="$varCommision"/>
										</xsl:when>

										<xsl:when test ="$varCommision &gt;0">
											<xsl:value-of select ="$varCommision*-1"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select ="0"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>

										<xsl:when test ="$varCommision &lt;0">
											<xsl:value-of select ="$varCommision*-1"/>
										</xsl:when>

										<xsl:when test ="$varCommision &gt;0">
											<xsl:value-of select ="$varCommision"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select ="0"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>

						</Commission>

						<xsl:variable name ="Fees">
							<xsl:value-of select ="COL33"/>
						</xsl:variable>

						<Fees>
							<xsl:choose>
								<xsl:when test="contains(COL27,'Cancel')">
									<xsl:choose>

										<xsl:when test ="$Fees &lt;0">
											<xsl:value-of select ="$Fees"/>
										</xsl:when>

										<xsl:when test ="$Fees &gt;0">
											<xsl:value-of select ="$Fees*-1"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select ="0"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>

										<xsl:when test ="$Fees &lt;0">
											<xsl:value-of select ="$Fees*-1"/>
										</xsl:when>

										<xsl:when test ="$Fees &gt;0">
											<xsl:value-of select ="$Fees"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select ="0"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>

						</Fees>

						<xsl:variable name="Multiplier" select="COL17"/>

						<Multiplier>

							<xsl:choose>
								<xsl:when test="$PRANA_MultiplierToExclude!=''">
									<xsl:value-of select="$PRANA_MultiplierToExclude"/>
								</xsl:when>
								<xsl:when test="$PRANA_Multiplier!=''">
									<xsl:value-of select="$Multiplier div $PRANA_Multiplier"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$Multiplier"/>
								</xsl:otherwise>
							</xsl:choose>

						</Multiplier>
						<!--<CurrencySymbol>
							<xsl:value-of select="COL4"/>
						</CurrencySymbol>-->

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>