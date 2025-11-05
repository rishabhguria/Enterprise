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




	<xsl:template name="spaces">
		<xsl:param name="count"/>
		<xsl:if test="number($count)">
			<xsl:call-template name="spaces">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:value-of select="' '"/>
	</xsl:template>



	<xsl:template match="/">


		<DocumentElement>

			<xsl:for-each select ="//Comparision">


				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL22"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Quantity) and COL2='T'">
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'ABN Amro Clearing'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL219)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>




						<xsl:variable name="Symbol" >
							<xsl:value-of select="''"/>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>





						<xsl:variable name="PB_FUND_NAME" select="COL11"/>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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




						<Quantity>
							<xsl:choose>
								<xsl:when test="number($Quantity)">
									<xsl:value-of select="$Quantity"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>



						<xsl:variable name="Underlyer" select="COL13"/>

						<xsl:variable name="Prana_Multiplier">
							<xsl:value-of select ="document('../ReconMappingXML/PriceMulMapping.xml')/PriceMulMapping/PB[@Name=$PB_NAME]/MultiplierData[@PranaRoot=$Underlyer]/@Multiplier"/>
						</xsl:variable>

						<xsl:variable name="AvgPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL20"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="Cost">
							<xsl:choose>
								<xsl:when test="number($Prana_Multiplier)">
									<xsl:value-of select="$AvgPrice div $Prana_Multiplier"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$AvgPrice"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<AvgPX>
							<xsl:choose>
								<xsl:when test="$AvgPrice &gt; 0">
									<xsl:value-of select="$AvgPrice"/>
								</xsl:when>
								<xsl:when test="$AvgPrice &lt; 0">
									<xsl:value-of select="$AvgPrice * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>



						<xsl:variable name="Side" select="COL21"/>
						<Side>

							<xsl:choose>
								<xsl:when test="$Side='1'">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="$Side='2'">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>

						<xsl:variable name="Month">
							<xsl:value-of select="substring(COL19,5,2)"/>
						</xsl:variable>
						<xsl:variable name="Day">
							<xsl:value-of select="substring(COL19,7,2)"/>
						</xsl:variable>
						<xsl:variable name="Year">
							<xsl:value-of select="substring(COL19,1,4)"/>
						</xsl:variable>
						<TradeDate>

							<xsl:value-of select ="concat($Month,'/',$Day,'/',$Year)"/>
						</TradeDate>

						<xsl:variable name="COL78">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL78"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="COL86">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL86"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="COL82">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL82"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="COL90">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL90"/>
							</xsl:call-template>
						</xsl:variable>
						

						<xsl:variable name="COL115">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL115"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="$COL78 + $COL86 + $COL82 + $COL90 + $COL115"/>
							</xsl:call-template>
						</xsl:variable>
						<TotalCommissionandFees>
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
						</TotalCommissionandFees>


						<xsl:variable name="COL69">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL69"/>
							</xsl:call-template>
						</xsl:variable>



						<xsl:variable name="NetNotionalValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="$COL69 + $COL78 + $COL86 + $COL82 + $COL90 + $COL115"/>
							</xsl:call-template>
						</xsl:variable>
						<NetNotionalValue>
							<xsl:choose>
								<xsl:when test="$NetNotionalValue &gt; 0">
									<xsl:value-of select="$NetNotionalValue"/>
								</xsl:when>
								<xsl:when test="$NetNotionalValue &lt; 0">
									<xsl:value-of select="$NetNotionalValue * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValue>

						<!--<xsl:variable name="NetNotionalValueBase">
							<xsl:value-of select="COL15"/>
						</xsl:variable>
						<NetNotionalValueBase>
							<xsl:choose>
								<xsl:when test="$NetNotionalValueBase &gt; 0">
									<xsl:value-of select="$NetNotionalValueBase"/>
								</xsl:when>
								<xsl:when test="$NetNotionalValueBase &lt; 0">
									<xsl:value-of select="$NetNotionalValueBase * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValueBase>-->


						<SettlementDate>
							<xsl:value-of select="''"/>
						</SettlementDate>


						<!--<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>-->

						<SMRequest>
							<xsl:value-of select="'true'"/>
						</SMRequest>

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>