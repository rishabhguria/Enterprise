<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

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

		<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month=01">
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
				<xsl:when test="$Month=11">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month=12">
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
		<xsl:variable name="UnderlyingSymbol">
			<xsl:value-of select="substring-before(normalize-space(COL39),' ')"/>
		</xsl:variable>
		<xsl:variable name="ExpiryDay">
			<xsl:value-of select="substring(substring-after(normalize-space(COL39),' '),5,2)"/>
		</xsl:variable>
		<xsl:variable name="ExpiryMonth">
			<xsl:value-of select="substring(substring-after(normalize-space(COL39),' '),3,2)"/>
		</xsl:variable>
		<xsl:variable name="ExpiryYear">
			<xsl:value-of select="substring(substring-after(normalize-space(COL39),' '),1,2)"/>
		</xsl:variable>
		<xsl:variable name="PutORCall">
			<xsl:value-of select="substring(substring-after(normalize-space(COL39),' '),7,1)"/>
		</xsl:variable>
		<xsl:variable name="StrikePrice">
			<xsl:value-of select="format-number(substring(substring-after(normalize-space(COL39),' '),8) div 1000,'##.00')"/>
		</xsl:variable>
		<xsl:variable name="MonthCodeVar">
			<xsl:call-template name="MonthCode">
				<xsl:with-param name="Month" select="($ExpiryMonth)"/>
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
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL4)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL2)"/>

				<xsl:if test="number($Position) and ($PB_FUND_NAME ='71240408' or $PB_FUND_NAME ='71240526' 
				or $PB_FUND_NAME ='71240527' or $PB_FUND_NAME ='71240528' or $PB_FUND_NAME ='71240529' or $PB_FUND_NAME ='71240540' 
				or $PB_FUND_NAME ='72711621' or $PB_FUND_NAME ='41954500' or $PB_FUND_NAME ='64860812' or $PB_FUND_NAME ='10111644' 
				or $PB_FUND_NAME ='1PB10097' )">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Wedbush'"/>
						</xsl:variable>

						<xsl:variable name = "PB_COMPANY_NAME">
							<xsl:value-of select="normalize-space(COL18)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>

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

						<PBSymbol>
							<xsl:value-of select="normalize-space(COL18)" />
						</PBSymbol>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="normalize-space(COL3) = 'P'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL3) = 'S'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL3) = 'SS'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
							</xsl:choose>
						</SideTagValue>

						<xsl:variable name="varAsset">
                          <xsl:choose>
								<xsl:when test="normalize-space(COL35) = 'P' or normalize-space(COL35) = 'C'">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
						        <xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>						
						</xsl:variable>
						
						<xsl:variable name="varSymbol">                         
							<xsl:value-of select="normalize-space(COL5)"/>						
						</xsl:variable>


						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varAsset='EquityOption'">
									<xsl:call-template name="Option">
										<xsl:with-param name="Symbol" select="normalize-space(COL39)"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:when test="$varSymbol!=''">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<xsl:variable name="CostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL6)"/>
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
								<xsl:with-param name="Number" select="normalize-space(COL15)"/>
							</xsl:call-template>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test="number($Commission)">
									<xsl:value-of select="$Commission"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name ="varSecFee">
							<xsl:choose>
								<xsl:when test ="number(COL12) ">
									<xsl:value-of select ="COL12"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<SecFee>
							<xsl:choose>
								<xsl:when test ="number($varSecFee) and $varSecFee &gt; 0">
									<xsl:value-of select ="$varSecFee"/>
								</xsl:when>
								<xsl:when test ="number($varSecFee) and $varSecFee &lt; 0">
									<xsl:value-of select ="$varSecFee * -1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SecFee>

						<xsl:variable name="varFees" select="number(COL13)"/>

						<Fees>
							<xsl:choose>
								<xsl:when test="normalize-space(COL2 = '88522033')">
									<xsl:choose>
										<xsl:when test ="number($varFees) and $varFees &gt; 0">
											<xsl:value-of select ="$varFees"/>
										</xsl:when>
										<xsl:when test ="number($varFees) and $varFees &lt; 0">
											<xsl:value-of select ="$varFees * -1"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select ="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
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

						<CounterPartyID>
							<xsl:value-of select ="122"/>
						</CounterPartyID>

						<OriginalPurchaseDate>
							<xsl:value-of select="COL28"/>
						</OriginalPurchaseDate>

						<PositionStartDate>
							<xsl:value-of select="normalize-space(COL26)"/>
						</PositionStartDate>

						<PositionSettlementDate>
							<xsl:value-of select="normalize-space(COL27)"/>
						</PositionSettlementDate>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


