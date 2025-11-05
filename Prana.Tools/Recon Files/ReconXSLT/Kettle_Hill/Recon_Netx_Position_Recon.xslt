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
				<xsl:when test="$Month=MAY">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month=JUN">
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
		<xsl:if test="contains(substring(substring-after(substring-after(substring-after(substring-after(COL22,' '),' '),' '),' '),1,1),P) or contains(substring(substring-after(substring-after(substring-after(substring-after(COL22,' '),' '),' '),' '),1,1),C) ">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="concat(substring(substring-before(COL22,' '),1,1),substring(substring-before(COL22,' '),3,1),substring(substring-before(substring-after(substring-after(COL22,' '),' '),' '),5,1),substring(substring-before(substring-after(COL22,' '),' '),1,1))"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(COL22,' '),' '),' '),' '),' '),' '),' '),' '),' '),1,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(COL22,' '),' '),' '),' '),' '),' '),' '),' '),' '),4,3)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(COL22,' '),' '),' '),' '),' '),' '),' '),' '),' '),10,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-after(substring-after(substring-after(substring-after(COL22,' '),' '),' '),' '),1,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring(substring-before(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(COL22,' '),' '),' '),' '),' '),' '),' '),7),'#.00')"/>

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
			<!--<xsl:variable name="ThirdFriday">
				<xsl:choose>
					<xsl:when test="$ExpiryMonth and number($ExpiryYear)">
						<xsl:value-of select="my:Now(number($ExpiryYear),$ExpiryMonth)"/>
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
			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
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

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">
				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL6"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="number($Quantity)">
					<PositionMaster>
						
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Pershing'"/>
						</xsl:variable>
						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL5)"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						

						<xsl:variable name="Symbol">
							<xsl:choose>
								<xsl:when test="COL4 = 'CORPORATE BOND'">
									<xsl:value-of select="COL3"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL2"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

					
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<xsl:variable name="PB_FUND_NAME" select="COL1"/>
						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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



					

						<xsl:variable name="MarketValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL9"/>
							</xsl:call-template>
						</xsl:variable>
						<MarketValueBase>
							<xsl:choose>
								<xsl:when test="number($MarketValue)">
									<xsl:value-of select="$MarketValue"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValueBase>

						<Side>
							<xsl:choose>
								<xsl:when test="$Quantity &gt; 0 and COL14='Margin'">							
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="$Quantity &lt; 0 and COL14='Short Sale'">
									<xsl:value-of select="'Short Sale'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<PBSymbol>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>

						<xsl:variable name="Year" select="substring(COL10,1,4)"/>
						<xsl:variable name="Month" select="substring(COL10,5,2)"/>
						<xsl:variable name="Day" select="substring(COL10,8,2)"/>
						<TradeDate>
							<xsl:value-of select="''"/>
						</TradeDate>

						<BaseCurrency>
							<xsl:value-of select="''"/>
						</BaseCurrency>
						<SettlCurrency>
							<xsl:value-of select="''"/>
						</SettlCurrency>

						<SMRequest>
							<xsl:value-of select="'true'"/>
						</SMRequest>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>