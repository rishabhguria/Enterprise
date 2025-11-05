<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'+',''))"/>
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

	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month=1 ">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month=2 ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month=3 ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month=4 ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month=5 ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month=6 ">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month=7  ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month=8  ">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month=9 ">
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
				<xsl:when test="$Month=1 ">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month=2 ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month=3 ">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month=4 ">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month=5 ">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month=6 ">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month=7  ">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month=8  ">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month=9 ">
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
		<xsl:if test="contains(substring(COL1,656,1),C) or contains(substring(COL1,656,1),P)">

			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring(COL1,657,3)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring(COL1,664,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring(COL1,662,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(COL1,660,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(COL1,656,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring(COL1,667,3),'#.00')"/>
			</xsl:variable>


			<xsl:variable name="MonthCodeVar">
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

			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>

		</xsl:if>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select ="//Comparision">


				<xsl:variable name ="varQuantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="substring(COL1,64,20)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test ="$varQuantity and not(contains(substring(COL1,357,21),'cash'))">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Triad'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="substring(COL1,28,35)"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="Symbol" select="substring(COL1,357,21)"/>

						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="substring(COL1,369,1) ='C' or (substring(COL1,369,1)='P')">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Asset='EquityOption'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$Asset='Equity'">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>


						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$Asset='EquityOption'">
									<xsl:value-of select="concat(substring(COL1,357,21),'U')"/>
								</xsl:when>

								<xsl:when test="$Asset='Equity'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</IDCOOptionSymbol>


						<xsl:variable name="PB_FUND_NAME" select="substring(COL1,1,15)"/>
						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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


						<xsl:variable name="Quantity">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="substring(COL1,64,20)"/>
							</xsl:call-template>
						</xsl:variable>
						<Quantity>
							<xsl:choose>
								<xsl:when  test="number($Quantity)">
									<xsl:value-of select="$Quantity"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>



						<xsl:variable name="MarketValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="substring(COL1,105,19)"/>
							</xsl:call-template>
						</xsl:variable>
						<MarketValue>
							<xsl:choose>
								<xsl:when test="$MarketValue &gt; 0">
									<xsl:value-of select="$MarketValue"/>
								</xsl:when>
								<xsl:when test="$MarketValue &lt; 0">
									<xsl:value-of select="$MarketValue * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarketValue>


						<xsl:variable name="MarkPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="$MarketValue div $Quantity"/>
							</xsl:call-template>
						</xsl:variable>
						<MarkPrice>
							<xsl:choose>
								<xsl:when test="$MarkPrice &gt; 0">
									<xsl:value-of select="$MarkPrice"/>

								</xsl:when>
								<xsl:when test="$MarkPrice &lt; 0">
									<xsl:value-of select="$MarkPrice * (1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarkPrice>

						<xsl:variable name="AvgPX">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="substring(COL1,210,18)"/>
							</xsl:call-template>
						</xsl:variable>
						<AvgPX>
							<xsl:choose>
								<xsl:when test="$AvgPX &gt; 0">
									<xsl:value-of select="$AvgPX"/>

								</xsl:when>
								<xsl:when test="$AvgPX &lt; 0">
									<xsl:value-of select="$AvgPX * (1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</AvgPX>

						<Side>
							<xsl:choose>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:choose>
										<xsl:when  test="number($varQuantity) &gt; 0">
											<xsl:value-of select="'Buy to Open'"/>
										</xsl:when>
										<xsl:when test="number($varQuantity) &lt; 0">
											<xsl:value-of select="'Sell to Close'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when  test="number($varQuantity) &gt; 0">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>
										<xsl:when test="number($varQuantity) &lt; 0">
											<xsl:value-of select="'Sell'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</Side>


						<xsl:variable name="Year">
							<xsl:value-of select="substring(COL1,378,4)"/>
						</xsl:variable>
						<xsl:variable name="Month">
							<xsl:value-of select="substring(COL1,383,2)"/>
						</xsl:variable>
						<xsl:variable name="Day">
							<xsl:value-of select="substring(COL1,386,2)"/>
						</xsl:variable>
						<TradeDate>
							<xsl:value-of select ="concat($Month,'/',$Day,'/',$Year)"/>
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

