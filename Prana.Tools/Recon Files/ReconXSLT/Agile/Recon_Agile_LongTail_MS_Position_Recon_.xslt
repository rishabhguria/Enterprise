<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber * (-1)"/>
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
				<xsl:when test="$Month=08 ">
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
		<xsl:if test="contains(substring-before(COL51,' '),'Put') or contains(substring-before(COL51,' '),'Call')">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="normalize-space(COL24)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring-before(substring-after(COL86,'/'),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring-before(COL86,'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after(substring-after(normalize-space(COL86),'/'),'/'),3,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:choose>
					<xsl:when test="contains(COL51,'Call')">
						<xsl:value-of select="'C'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'P'"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring-before(substring-after(COL84,' '),' '),'#.00')"/>
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

			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL28"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Position)">
					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'GS'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL24"/>
						</xsl:variable>

						<xsl:variable name="varCurrency">
							<xsl:value-of select="COL44"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="contains(substring-before(COL51,' '),'Put') or contains(substring-before(COL51,' '),'Call')">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:when test="COL51='Futures'">
									<xsl:value-of select="'Future'"/>
								</xsl:when>
								<xsl:when test="COL50='EQTY'">
									<xsl:value-of select="'Equity'"/>
								</xsl:when>
								<xsl:when test="COL50='USTRS'">
									<xsl:value-of select="'FixedIncome'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<Asset>
							<xsl:value-of select="$Asset"/>
						</Asset>

						<xsl:variable name="Root_Code">
							<xsl:choose>
					     		<xsl:when test="string-length(COL14)= 5">
									<xsl:value-of select="substring(COL14,1,3)"/>
								</xsl:when>
								<xsl:when test="contains(COL14,' ')">
									<xsl:value-of select="substring-before(COL14,' ')"/>
								</xsl:when>
								<xsl:when test="string-length(COL14)= 4">
									<xsl:value-of select="substring(COL14,1,2)"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="MONTH_CODE">
							<xsl:choose>
								<xsl:when test="string-length(COL14)= 5">
									<xsl:value-of select="substring(COL14,4,1)"/>
								</xsl:when>
								<xsl:when test="string-length(COL14)= 4">
									<xsl:value-of select="substring(COL14,3,1)"/>
								</xsl:when>
								<xsl:when test="contains(COL14,' ')">
									<xsl:value-of select="substring(substring-after(COL14,' '),1,1)"/>
								</xsl:when>

							</xsl:choose>
						</xsl:variable>



						<xsl:variable name ="Year">
							<xsl:choose>
								<xsl:when test="string-length(normalize-space(COL14))= 5">
									<xsl:value-of select="substring(COL14,5,1)"/>
								</xsl:when>
								<xsl:when test="string-length(normalize-space(COL14))= 4">
									<xsl:value-of select="substring(COL14,4,1)"/>
								</xsl:when>
								<xsl:when test="contains(COL14,' ')">
									<xsl:value-of select="substring(substring-after(COL14,' '),2,1)"/>
								</xsl:when>

							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="PRANA_UNDERLYING_NAME">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$Root_Code]/@UnderlyingCode"/>
						</xsl:variable>


						<xsl:variable name="PRANA_EXCHANGE_NAME">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$Root_Code]/@ExchangeCode"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_Multiplier">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$Root_Code]/@PriceMul"/>
						</xsl:variable>

						<xsl:variable name="varUnderlying">
							<xsl:choose>
								<xsl:when test="$PRANA_UNDERLYING_NAME!=''">
									<xsl:value-of select="$PRANA_UNDERLYING_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$Root_Code"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="varExchange">
							<xsl:choose>
								<xsl:when test="$PRANA_EXCHANGE_NAME!=''">
									<xsl:value-of select="$PRANA_EXCHANGE_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

					<xsl:variable name="Symbol" select="COL14"/>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

															
								<xsl:when test="$Asset='Future'">
									<xsl:value-of select="concat($varUnderlying,' ',$MONTH_CODE,$Year,$varExchange)"/>
								</xsl:when>

								<!--<xsl:when test="$Asset='FutureOption'">
									<xsl:value-of select="concat($varFUTUnderlyng,' ',$varFutCode,$varFutOptStrikePrice)"/>
								</xsl:when>-->

								<xsl:when test="$Asset='FixedIncome'">
									<xsl:value-of select="COL7"/>
								</xsl:when>

								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>


						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL4)"/>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
								<xsl:when test="number($Position)">
									<xsl:value-of select="$Position"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>


						<xsl:variable name="Side">
							<xsl:value-of select="COL29"/>
						</xsl:variable>

						<Side>
							<xsl:choose>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:choose>
										<xsl:when  test="$Side='L'">
											<xsl:value-of select="'Buy to Open'"/>
										</xsl:when>
										<xsl:when  test="$Side='S'">
											<xsl:value-of select="'Sell to Open'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when  test="$Side='L'">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>
										<xsl:when  test="$Side='S'">
											<xsl:value-of select="'Sell Short'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
							
							
						</Side>


						<xsl:variable name="MarkPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL30"/>
							</xsl:call-template>
						</xsl:variable>


						<xsl:variable name="varMarkPrice">
							<xsl:choose>

								<xsl:when test ="$MarkPrice &lt;0">
									<xsl:value-of select ="$MarkPrice*-1"/>
								</xsl:when>

								<xsl:when test ="$MarkPrice &gt;0">
									<xsl:value-of select ="$MarkPrice"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>
								<xsl:when test="number($PRANA_Multiplier)">
									<xsl:value-of select="$varMarkPrice * $PRANA_Multiplier"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varMarkPrice"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>

						

						<xsl:variable name="FXRate">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL67"/>
							</xsl:call-template>
						</xsl:variable>
						<FXRate>
							<xsl:choose>
								<xsl:when test="$FXRate &gt; 0">
									<xsl:value-of select="$FXRate"/>
								</xsl:when>
								<xsl:when test="$FXRate &lt; 0">
									<xsl:value-of select="$FXRate * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</FXRate>

						<CurrencySymbol>
							<xsl:value-of select="COL44"/>
						</CurrencySymbol>


						<TradeDate>
							<xsl:value-of select="''"/>
						</TradeDate>


						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>