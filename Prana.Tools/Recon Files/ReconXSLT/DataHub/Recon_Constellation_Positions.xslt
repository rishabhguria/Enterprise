<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

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


	<xsl:template name="GetSuffix">
		<xsl:param name="Suffix"/>
		<xsl:choose>
			<xsl:when test="$Suffix = 'JPY'">
				<xsl:value-of select="'-TSE'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'CHF'">
				<xsl:value-of select="'-SWX'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'EUR'">
				<xsl:value-of select="'-EEB'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'CAD'">
				<xsl:value-of select="'-TC'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
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
				<xsl:when test="$Month=02">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month=03">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month=04">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month=05">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month=06">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month=07">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month=08">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month=09">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month=10">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month=11">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month=12">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month=01">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month=02">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month=03">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month=04">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month=05">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month=06">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month=07">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month=08">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month=09">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month=10">
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

	<xsl:template name="MonthsCode">
		<xsl:param name="varMonth"/>
		<xsl:choose>
			<xsl:when test="$varMonth=01">
				<xsl:value-of select="'F'"/>
			</xsl:when>
			<xsl:when test="$varMonth=02">
				<xsl:value-of select="'G'"/>
			</xsl:when>
			<xsl:when test="$varMonth=03">
				<xsl:value-of select="'H'"/>
			</xsl:when>
			<xsl:when test="$varMonth=04">
				<xsl:value-of select="'J'"/>
			</xsl:when>
			<xsl:when test="$varMonth=05">
				<xsl:value-of select="'K'"/>
			</xsl:when>
			<xsl:when test="$varMonth=06">
				<xsl:value-of select="'M'"/>
			</xsl:when>
			<xsl:when test="$varMonth=07">
				<xsl:value-of select="'N'"/>
			</xsl:when>
			<xsl:when test="$varMonth=08">
				<xsl:value-of select="'Q'"/>
			</xsl:when>
			<xsl:when test="$varMonth=09">
				<xsl:value-of select="'U'"/>
			</xsl:when>
			<xsl:when test="$varMonth=10">
				<xsl:value-of select="'V'"/>
			</xsl:when>
			<xsl:when test="$varMonth=11">
				<xsl:value-of select="'X'"/>
			</xsl:when>
			<xsl:when test="$varMonth=12">
				<xsl:value-of select="'Z'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="contains(substring-before(COL4,' '),'CALL') or contains(substring-before(COL4,' '),'PUT')">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(COL19,' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring(substring-after(normalize-space(COL19),' '),5,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring(substring-after(normalize-space(COL19),' '),3,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after(normalize-space(COL19),' '),1,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-after(normalize-space(COL19),' '),7,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring-before(substring-after(substring-after(substring-after(COL4,' '),' '),' '),' '),'#.00')"/>
			</xsl:variable>


			<xsl:variable name="MonthCodeVar">
				<xsl:call-template name="MonthCode">
					<xsl:with-param name="Month" select="number($ExpiryMonth)"/>
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

			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>

		</xsl:if>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//Comparision">
				<xsl:if test="number(COL8) and normalize-space(COL6) != 'Cash and Equivalents'">

					<PositionMaster>
						<!--   Fund -->
						<!--fundname section-->
						<xsl:variable name="varPBName">
							<xsl:value-of select="'Jeff'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL3"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:variable name = "PB_Symbol_NAME" >
							<xsl:value-of select="normalize-space(COL4)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_Symbol_NAME">
							<xsl:value-of select="document('../../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="varPositionStartDate">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varQuantity">
							<xsl:value-of select="COL8"/>
						</xsl:variable>

						<xsl:variable name="varOptionSymbol">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varEquitySymbol">
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<xsl:variable name="varFutureSymbol">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varCUSIP">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varRIC">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varBloomberg">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varSEDOL">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varOSISymbol">
							<xsl:value-of select="COL19"/>
						</xsl:variable>

						<xsl:variable name="varOptionExpiry">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varPBSymbol">
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<xsl:variable name="CompanyName">
							<xsl:value-of select="COL4"/>
						</xsl:variable>

						<xsl:variable name="varMarkPrice">
							<xsl:value-of select="COL13"/>
						</xsl:variable>
						<xsl:variable name="varNetNotionalValue">
							<xsl:value-of select="COL11"/>
						</xsl:variable>

						<xsl:variable name="varNetNotionalValueBase">
							<xsl:value-of select="COL10"/>
						</xsl:variable>


						<xsl:variable name="varCounterPartyID">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varAssetType">
							<xsl:value-of select="COL6"/>
						</xsl:variable>

						<xsl:variable name="varCommission">
							<xsl:value-of select="0"/>
						</xsl:variable>

						<xsl:variable name="varMarketValue">
							<xsl:value-of select="COL17"/>
						</xsl:variable>

						<xsl:variable name="varMarketValueBase">
							<xsl:value-of select="COL16"/>
						</xsl:variable>



						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<FundName>
									<xsl:value-of select='$PB_FUND_NAME'/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</FundName>
							</xsl:otherwise>
						</xsl:choose>

						<PositionStartDate>
							<xsl:value-of select="$varPositionStartDate"/>
						</PositionStartDate>

						<xsl:variable name="PB_SUFFIX_NAME">
							<xsl:value-of select="COL5"/>
						</xsl:variable>


						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../../ReconMappingXML/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
						</xsl:variable>
						
						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="string-length(COL19 &gt; 20)">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:when test="COL6='Equity'">
									<xsl:value-of select="'Equity'"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="'FixedIncome'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>



						<xsl:variable name="varSymbol">
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_Symbol_NAME != ''">
									<xsl:value-of select="$PRANA_Symbol_NAME"/>
								</xsl:when>

								<xsl:when test="$varAssetType='Options'">
									<xsl:call-template name="Option">
										<xsl:with-param name="Symbol" select="COL19"/>
										<xsl:with-param name="Suffix" select="''"/>
									</xsl:call-template>
								</xsl:when>
								
								<xsl:when test="$varAssetType='Equity'">
									<xsl:value-of select="concat($varSymbol,$PRANA_SUFFIX_NAME)"/>
								</xsl:when>



								<xsl:when test="$varAssetType='FixedIncome'">
						             <xsl:value-of select="COL20"/>
					            </xsl:when>

					       
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>


					

						<PBSymbol>
							<xsl:value-of select="$varPBSymbol"/>
						</PBSymbol>

						<CompanyName>
							<xsl:value-of select="$CompanyName"/>
						</CompanyName>

						<Quantity>
							<xsl:value-of select="$varQuantity"/>
						</Quantity>




						<MarkPrice>
							<xsl:choose>
								<xsl:when test ="boolean(number($varMarkPrice))">
									<xsl:value-of select="$varMarkPrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>



						<MarketValue>
							<xsl:choose>
								<xsl:when test ="number($varMarketValue) ">
									<xsl:value-of select="$varMarketValue"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValue>

						<MarketValueBase>
							<xsl:choose>
								<xsl:when test ="number($varMarketValueBase) ">
									<xsl:value-of select="$varMarketValueBase"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValueBase>
						<NetNotionalValue>
							<xsl:choose>
								<xsl:when test ="number($varNetNotionalValue) ">
									<xsl:value-of select="$varNetNotionalValue"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValue>

						<NetNotionalValueBase>
							<xsl:choose>
								<xsl:when test ="number($varNetNotionalValueBase) ">
									<xsl:value-of select="$varNetNotionalValueBase"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValueBase>

						<CUSIPSymbol>
							<xsl:value-of select="COL20"/>
						</CUSIPSymbol>

						<SEDOLSymbol>
				  <xsl:value-of select="COL19"/>
			  </SEDOLSymbol>

						<ISINSymbol>
							<xsl:value-of select="COL21"/>
						</ISINSymbol>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
