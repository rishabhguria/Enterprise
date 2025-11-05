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
		<xsl:if test="COL12='OPTION' and (COL34='INDEX' or COL34='ETF')">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(COL14,' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring-before(substring-after(COL14,'/'),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL14),' '),' '),' '),' '),' '),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after(substring-after(normalize-space(COL14),'/'),'/'),3,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-before(substring-after(substring-after(substring-after(substring-after(normalize-space(COL14),' '),' '),' '),' '),' '),1,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring-before(substring-after(normalize-space(COL14),' '),' '),'00.00')"/>
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
						<xsl:with-param name="Number" select="COL23"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Position) and COL12!='CURRENCY' ">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'NT'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL14"/>
						</xsl:variable>

						<xsl:variable name="varCurrency">
							<xsl:value-of select="COL20"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='NT']/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="COL12='ETF' or COL12='COMMON' or COL12='INTEREST_RATE_SWAP'">
									<xsl:value-of select="'Equity'"/>
								</xsl:when>
								<xsl:when test="COL12='FUTURES'">
									<xsl:value-of select="'Future'"/>
								</xsl:when>
								<xsl:when test="COL12='OPTION' and COL34='FUTURES'">
									<xsl:value-of select="'FutureOption'"/>
								</xsl:when>
								<xsl:when test="COL12='OPTION' and (COL34='INDEX' or COL34='ETF')">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:when test="COL12='GOVT BOND' and (COL34='' or COL43='*')">
									<xsl:value-of select="'FixedIncome'"/>
								</xsl:when>
								<xsl:when test="COL12='INTEREST_RATE_SWAP' and COL34='INTEREST'">
									<xsl:value-of select="'PrivateEquity'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<Asset>
							<xsl:value-of select="$Asset"/>
						</Asset>
						
						<xsl:variable name="Root_Code">
							<xsl:choose>
								<xsl:when test="string-length(COL19)= 5 and $Asset='FutureOption'">
									<xsl:value-of select="substring(COL19,1,2)"/>
								</xsl:when>

								<xsl:when test="string-length(COL19)= 5 and $Asset='Future'">
									<xsl:value-of select="substring(COL19,1,3)"/>
								</xsl:when>
								<xsl:when test="contains(COL19,' ')">
									<xsl:value-of select="substring-before(COL19,' ')"/>
								</xsl:when>
								<xsl:when test="string-length(COL19)= 4">
									<xsl:value-of select="substring(COL19,1,2)"/>
								</xsl:when>

							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="MONTH_CODE">
							<xsl:choose>
								<xsl:when test="string-length(COL19)= 5">
									<xsl:value-of select="substring(COL19,4,1)"/>
								</xsl:when>
								<xsl:when test="string-length(COL19)= 4">
									<xsl:value-of select="substring(COL19,3,1)"/>
								</xsl:when>
								<xsl:when test="contains(COL19,' ')">
									<xsl:value-of select="substring(substring-after(COL19,' '),1,1)"/>
								</xsl:when>

							</xsl:choose>
						</xsl:variable>



						<xsl:variable name ="Year">
							<xsl:choose>
								<xsl:when test="string-length(normalize-space(COL19))= 6 and substring(COL19,6,1)='8'">
									<xsl:value-of select="concat('1',substring(COL19,6,1))"/>
								</xsl:when>
								<xsl:when test="string-length(normalize-space(COL19))= 6">
									<xsl:value-of select="substring(COL19,6,1)"/>
								</xsl:when>
								<xsl:when test="string-length(normalize-space(COL19))= 5 and substring(COL19,5,1)='8'">
									<xsl:value-of select="concat('1',substring(COL19,5,1))"/>
								</xsl:when>
								<xsl:when test="string-length(normalize-space(COL19))= 5">
									<xsl:value-of select="substring(COL19,5,1)"/>
								</xsl:when>
								<xsl:when test="string-length(normalize-space(COL19))= 4 and substring(COL19,4,1)='8'">
									<xsl:value-of select="concat('1',substring(COL19,4,1))"/>
								</xsl:when>
								<xsl:when test="string-length(normalize-space(COL19))= 4">
									<xsl:value-of select="substring(COL19,4,1)"/>
								</xsl:when>
								<xsl:when test="string-length(normalize-space(COL19))= 3 and substring(COL19,3,1)='8'">
									<xsl:value-of select="concat('1',substring(COL19,3,1))"/>
								</xsl:when>
								<xsl:when test="string-length(normalize-space(COL19))= 3">
									<xsl:value-of select="substring(COL19,3,1)"/>
								</xsl:when>
								<xsl:when test="contains(COL19,' ') and substring(substring-after(COL19,' '),2,1)='8'">
									<xsl:value-of select="concat('1',substring(substring-after(COL19,' '),2,1))"/>
								</xsl:when>
								<xsl:when test="contains(COL19,' ')">
									<xsl:value-of select="substring(substring-after(COL19,' '),2,1)"/>
								</xsl:when>

							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="PRANA_UNDERLYING_NAME">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$Root_Code and @Currency=$varCurrency]/@UnderlyingCode"/>
						</xsl:variable>


						<xsl:variable name="PRANA_EXCHANGE_NAME">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$Root_Code and @Currency=$varCurrency]/@ExchangeCode"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_Multiplier">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$Root_Code and @Currency=$varCurrency]//@PriceMul"/>
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

						<xsl:variable name="varFutStrickPrice">
							<xsl:value-of select="substring-before(substring-after(COL14,' '),' ')"/>
						</xsl:variable>


						<xsl:variable name="PRANA_STRIKE_MULTIPLIER">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$Root_Code and @Currency=$varCurrency]/@StrikeMul"/>
						</xsl:variable>

						<xsl:variable name="varFutOptStrikePrice">
							<xsl:choose>
								<xsl:when test="$PRANA_STRIKE_MULTIPLIER!=''">
									<xsl:value-of select="format-number(($varFutStrickPrice )* ($PRANA_STRIKE_MULTIPLIER),'0.##')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varFutStrickPrice"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>



						<xsl:variable name="varFUTUnderlyng">
							<xsl:choose>
								<xsl:when test="string-length(normalize-space(COL19))= 5">
									<xsl:value-of select="substring(normalize-space(COL19),1,2)"/>
								</xsl:when>
								<xsl:when test="string-length(normalize-space(COL19))= 4">
									<xsl:value-of select="substring(normalize-space(COL19),1,2)"/>
								</xsl:when>
								<xsl:when test="contains(COL29,' ')">
									<xsl:value-of select="substring(substring-after(COL19,' '),2,1)"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varFutCode">
							<xsl:choose>
								<xsl:when test="string-length(COL19)= 5">
									<xsl:value-of select="substring(COL19,3)"/>
								</xsl:when>
								<xsl:when test="string-length(COL19)= 4">
									<xsl:value-of select="substring(COL19,3)"/>
								</xsl:when>
								<xsl:when test="contains(COL19,' ')">
									<xsl:value-of select="substring(substring-after(COL19,' '),2,1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring(COL19,3)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						
						
						

						<xsl:variable name="Symbol" select="COL19"/>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Asset='EquityOption'">
									<xsl:call-template name="Option">
										<xsl:with-param name="Symbol" select="COL19"/>
										<xsl:with-param name="Suffix" select="''"/>
									</xsl:call-template>
								</xsl:when>


								<xsl:when test="$Symbol=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								
								<xsl:when test="$Asset='Future'">
									<xsl:value-of select="concat($varUnderlying,' ',$MONTH_CODE,$Year,$varExchange)"/>
								</xsl:when>

								<xsl:when test="$Asset='FutureOption'">
									<xsl:value-of select="concat($varFUTUnderlyng,' ',$varFutCode,$varFutOptStrikePrice)"/>
								</xsl:when>

								<xsl:when test="$Asset='FixedIncome'">
									<xsl:value-of select="COL15"/>
								</xsl:when>

								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<CUSIP>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$Asset='EquityOption'">
									<xsl:value-of select="''"/>
								</xsl:when>


								<xsl:when test="$Symbol=''">
									<xsl:value-of select="COL15"/>
								</xsl:when>

								<xsl:when test="$Asset='Future'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$Asset='FutureOption'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$Asset='FixedIncome'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</CUSIP>
						
						<xsl:variable name="PB_FUND_NAME" select="COL3"/>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/MasterFundMapping.xml')/MasterFundMapping/PB[@Name=$PB_NAME]/MasterFundData[@FundName=$PB_FUND_NAME]/@MasterFundName"/>
						</xsl:variable>

						<MasterFund>
							<xsl:choose>

								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</MasterFund>
						
						


						<xsl:variable name="varPosition">
							<xsl:choose>
								<xsl:when test="$Asset='FixedIncome'">
									<xsl:value-of select="$Position * 1000"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$Position"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Quantity>
							<xsl:choose>
								<xsl:when test="$varPosition &gt; 0">
									<xsl:value-of select="$varPosition"/>
								</xsl:when>
								<xsl:when test="$varPosition &lt; 0">
									<xsl:value-of select="$varPosition * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						

						<xsl:variable name="Side">
							<xsl:value-of select="substring-before(COL9,'_')"/>
						</xsl:variable>
						<Side>
							<xsl:choose>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:choose>
										<xsl:when  test="$Side='BUY'">
											<xsl:value-of select="'Buy to Open'"/>
										</xsl:when>
										<xsl:when  test="$Side='SELL'">
											<xsl:value-of select="'Sell to Close'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when  test="$Side='BUY'">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>
										<xsl:when  test="$Side='SELL'">
											<xsl:value-of select="'Sell'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<xsl:variable name="TradePrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL30 div (COL23 * COL24)"/>
							</xsl:call-template>
						</xsl:variable>



						<xsl:variable name="varCostBasis">
							<xsl:choose>

								<xsl:when test ="$TradePrice &lt;0">
									<xsl:value-of select ="$TradePrice*-1"/>
								</xsl:when>

								<xsl:when test ="$TradePrice &gt;0">
									<xsl:value-of select ="$TradePrice"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</xsl:variable>

						<AvgPX>
							<xsl:choose>
								<xsl:when test="number($PRANA_Multiplier)">
									<xsl:value-of select="$varCostBasis * $PRANA_Multiplier"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varCostBasis"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>


						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL28"/>
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


						<xsl:variable name="varNetNotionalValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL31"/>
							</xsl:call-template>
						</xsl:variable>
						
						<xsl:variable name="NetNotionalValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL30 - $Commission"/>
							</xsl:call-template>
						</xsl:variable>
						<NetNotionalValue>

							<xsl:choose>
								<xsl:when test="$Asset='EquityOption' or $Asset='FixedIncome'">
									<xsl:choose>
										<xsl:when test="$varNetNotionalValue &gt; 0">
											<xsl:value-of select="$varNetNotionalValue"/>
										</xsl:when>
										<xsl:when test="$varNetNotionalValue &lt; 0">
											<xsl:value-of select="$varNetNotionalValue * (-1)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
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
								</xsl:otherwise>
							</xsl:choose>
							
						
						</NetNotionalValue>

						<CurrencySymbol>
							<xsl:value-of select="COL20"/>
						</CurrencySymbol>

						<PBSymbol>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>

						<xsl:variable name="Date" select="COL1"/>
						<TradeDate>
							<xsl:value-of select="$Date"/>
						</TradeDate>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>