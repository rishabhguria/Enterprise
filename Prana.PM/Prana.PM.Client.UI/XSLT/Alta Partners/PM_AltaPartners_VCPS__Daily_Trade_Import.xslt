<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

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
				<xsl:when test="$Month=07 ">
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


	<xsl:template name="ConvertBBCodetoTicker">
		<xsl:param name="varBBCode"/>

		<xsl:variable name="varUSymbol">
			<xsl:value-of select="substring-before($varBBCode,' ')"/>
		</xsl:variable>

		<xsl:variable name="varPutORCall">
			<xsl:value-of select="substring(substring-after(normalize-space($varBBCode),' '),7,1)"/>
		</xsl:variable>

		<xsl:variable name="varExYear">
			<xsl:value-of select="substring(substring-after(normalize-space($varBBCode),' '),1,2)"/>
		</xsl:variable>

		<xsl:variable name="varStrike">

			<xsl:choose>
				<xsl:when test ="format-number(substring(substring-after(normalize-space($varBBCode),' '),8) div 1000,'##.00') &gt; 1">
					<xsl:value-of select ="format-number(substring(substring-after(normalize-space($varBBCode),' '),8) div 1000,'##.00')"/>
				</xsl:when>
				<xsl:when test ="format-number(substring(substring-after(normalize-space($varBBCode),' '),8) div 1000,'##.00') = 1">
					<xsl:value-of select ="format-number(substring(substring-after(normalize-space($varBBCode),' '),8) div 1000,'##.00')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="concat('0',format-number(substring(substring-after(normalize-space($varBBCode),' '),8) div 1000,'##.00'))"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varExDay">
			<xsl:value-of select="substring(substring-after(normalize-space($varBBCode),' '),5,2)"/>
		</xsl:variable>

		<xsl:variable name="varMonthCode">
			<xsl:value-of select="substring(substring-after(normalize-space($varBBCode),' '),3,2)"/>
		</xsl:variable>


		<xsl:variable name="MonthCodeVar">
			<xsl:call-template name="MonthCode">
				<xsl:with-param name="Month" select="$varMonthCode"/>
				<xsl:with-param name="PutOrCall" select="$varPutORCall"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name="varExpiryDay">
			<xsl:choose>
				<xsl:when test="substring($varExDay,1,1)= '0'">
					<xsl:value-of select="substring($varExDay,2,1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$varExDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:value-of select="normalize-space(concat('O:',$varUSymbol,' ',$varExYear,$MonthCodeVar,$varStrike,'D',$varExpiryDay))"/>
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

				<xsl:variable name="varPosition_COL9">
					<xsl:choose>
						<xsl:when  test="COL9 != '*' and COL9 != ''">
							<xsl:value-of select="COL9"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:if test="number($varPosition_COL9)">
					<!--<xsl:if test="number($Position) and (COL32='Buy' or COL32='Sell') and COL9!='CASH'">-->
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Velocity'"/>
						</xsl:variable>

						<xsl:variable name = "PB_COMPANY_NAME" >
							<xsl:value-of select="normalize-space(COL8)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="string-length(COL8) &gt; 10">
									<xsl:call-template name="ConvertBBCodetoTicker">
										<xsl:with-param name="varBBCode" select="COL8"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_COMPANY_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<PBSymbol>
							<xsl:value-of select="$PB_COMPANY_NAME"/>
						</PBSymbol>

						<xsl:variable name="PB_FUND_NAME">
							<xsl:choose>
								<xsl:when test="COL24='*' or COL24=''">
									<xsl:value-of select="concat(normalize-space(COL25),'-',normalize-space(COL26),'-',normalize-space(COL27),'-',normalize-space(COL28))"/>
								</xsl:when>
								<xsl:when test="COL25='*' or COL25=''">
									<xsl:value-of select="concat(normalize-space(COL24),'-',normalize-space(COL26),'-',normalize-space(COL27),'-',normalize-space(COL28))"/>
								</xsl:when>
								<xsl:when test="COL26='*' or COL26=''">
									<xsl:value-of select="concat(normalize-space(COL24),'-',normalize-space(COL25),'-',normalize-space(COL27),'-',normalize-space(COL28))"/>
								</xsl:when>
								<xsl:when test="COL27='*' or COL27=''">
									<xsl:value-of select="concat(normalize-space(COL24),'-',normalize-space(COL25),'-',normalize-space(COL26),'-',normalize-space(COL28))"/>
								</xsl:when>
								<xsl:when test="COL28='*' or COL28=''">
									<xsl:value-of select="concat(normalize-space(COL24),'-',normalize-space(COL25),'-',normalize-space(COL26),'-',normalize-space(COL27))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat(normalize-space(COL24),'-',normalize-space(COL25),'-',normalize-space(COL26),'-',normalize-space(COL27),'-',normalize-space(COL28))"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

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

						<xsl:variable name="varCostBasis_COL10">
							<xsl:choose>
								<xsl:when  test="COL10 != '*' and COL10 != ''">
									<xsl:value-of select="COL10"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<CostBasis>
							<xsl:choose>
								<xsl:when  test="$varCostBasis_COL10 &gt; 0">
									<xsl:value-of select="$varCostBasis_COL10"/>
								</xsl:when>
								<xsl:when test="$varCostBasis_COL10 &lt; 0">
									<xsl:value-of select="$varCostBasis_COL10 * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<xsl:variable name="varSecFee_COL13">
							<xsl:choose>
								<xsl:when  test="COL13 != '*' and COL13 != ''">
									<xsl:value-of select="COL13"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="varSecFee_COL14">
							<xsl:choose>
								<xsl:when  test="COL14 != '*' and COL14 != ''">
									<xsl:value-of select="COL14"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<SecFee>
							<xsl:choose>
								<xsl:when  test="$varSecFee_COL13 + $varSecFee_COL14 &gt; 0">
									<xsl:value-of select="$varSecFee_COL13 + $varSecFee_COL14 "/>
								</xsl:when>
								<xsl:when test="($varSecFee_COL13 + $varSecFee_COL14) &lt; 0">
									<xsl:value-of select="($varSecFee_COL13  + $varSecFee_COL14)* (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SecFee>

						<xsl:variable name="varCommission_COL12">
							<xsl:choose>
								<xsl:when  test="COL12 != '*' and COL12 != ''">
									<xsl:value-of select="COL12"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when  test="$varCommission_COL12 &gt; 0">
									<xsl:value-of select="$varCommission_COL12"/>
								</xsl:when>
								<xsl:when test="$varCommission_COL12 &lt; 0">
									<xsl:value-of select="$varCommission_COL12 * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="varOrffees_COL19">
							<xsl:choose>
								<xsl:when  test="COL19 != '*' and COL19 != ''">
									<xsl:value-of select="COL19"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<OrfFee>
							<xsl:choose>
								<xsl:when  test="$varOrffees_COL19 &gt; 0">
									<xsl:value-of select="$varOrffees_COL19"/>
								</xsl:when>
								<xsl:when test="$varOrffees_COL19 &lt; 0">
									<xsl:value-of select="$varOrffees_COL19 * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</OrfFee>

						<xsl:variable name="varOccFee_COL18">
							<xsl:choose>
								<xsl:when  test="COL18 != '*' and COL18 != ''">
									<xsl:value-of select="COL18"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

					
						<OccFee>
							<xsl:choose>
								<xsl:when test="number($varOccFee_COL18) and $varOccFee_COL18 &gt; 0">
									<xsl:value-of select="$varOccFee_COL18"/>
								</xsl:when>
								<xsl:when test="number($varOccFee_COL18) and $varOccFee_COL18 &lt; 0">
									<xsl:value-of select="$varOccFee_COL18 * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</OccFee>

						<xsl:variable name="varClearingFee_COL15">
							<xsl:choose>
								<xsl:when  test="COL15 != '*' and COL15 != ''">
									<xsl:value-of select="COL15"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
					<xsl:variable name="varClearingFee_COL16">
							<xsl:choose>
								<xsl:when  test="COL16 != '*' and COL16 != ''">
									<xsl:value-of select="COL16"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<ClearingFee>
							<xsl:choose>
								<xsl:when test="($varClearingFee_COL15+ $varClearingFee_COL16) &gt; 0">
									<xsl:value-of select="($varClearingFee_COL15+ $varClearingFee_COL16)"/>
								</xsl:when>
								
								<!-- remove -1 because they need as it -->
								<xsl:when test="($varClearingFee_COL15+ $varClearingFee_COL16) &lt; 0">
									<xsl:value-of select="($varClearingFee_COL15+ $varClearingFee_COL16) "/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</ClearingFee>
						
							<xsl:variable name="varAUECFee2_COL70">
							<xsl:choose>
								<xsl:when  test="COL70 != '*' and COL70 != ''">
									<xsl:value-of select="COL70"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<MiscFees>
							<xsl:choose>
								<xsl:when  test="$varAUECFee2_COL70 &gt; 0">
									<xsl:value-of select="$varAUECFee2_COL70"/>
								</xsl:when>
								<xsl:when test="$varAUECFee2_COL70 &lt; 0">
									<xsl:value-of select="$varAUECFee2_COL70* (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MiscFees>

						<xsl:variable name="varOtherBrokerFees_COL17">
							<xsl:choose>
								<xsl:when  test="COL17 != '*' and COL17 != ''">
									<xsl:value-of select="COL17"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varOtherFee_COL21">
							<xsl:choose>
								<xsl:when  test="COL21 != '*' and COL21 != ''">
									<xsl:value-of select="COL21"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<Fees>
							<xsl:choose>
								<xsl:when  test="($varOtherBrokerFees_COL17 + $varOtherFee_COL21) &gt; 0">
									<xsl:value-of select="($varOtherBrokerFees_COL17 + $varOtherFee_COL21)"/>
								</xsl:when>
								<xsl:when test="($varOtherBrokerFees_COL17 + $varOtherFee_COL21) &lt; 0">
									<xsl:value-of select="($varOtherBrokerFees_COL17 + $varOtherFee_COL21) * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>
						<xsl:variable name="varDay">
							<xsl:value-of select="substring-before(substring-after(COL5,'/'),'/')"/>
						</xsl:variable>

						<xsl:variable name="varYear">
							<xsl:value-of select="substring-after(substring-after(COL5,'/'),'/')"/>
						</xsl:variable>

						<xsl:variable name="varMonth">
							<xsl:value-of select="substring-before(COL5,'/')"/>
						</xsl:variable>
						<PositionStartDate>
							<xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
						</PositionStartDate>

						<xsl:variable name="varSDay">
							<xsl:value-of select="substring-before(substring-after(COL6,'/'),'/')"/>
						</xsl:variable>

						<xsl:variable name="varSYear">
							<xsl:value-of select="substring-after(substring-after(COL6,'/'),'/')"/>
						</xsl:variable>

						<xsl:variable name="varSMonth">
							<xsl:value-of select="substring-before(COL6,'/')"/>
						</xsl:variable>
						<PositionSettlementDate>
							<xsl:value-of select="concat($varSMonth,'/',$varSDay,'/',$varSYear)"/>
						</PositionSettlementDate>

						<OriginalPurchaseDate>
							<xsl:value-of select="''"/>
						</OriginalPurchaseDate>


						<NetPosition>
							<xsl:choose>
								<xsl:when  test="$varPosition_COL9 &gt; 0">
									<xsl:value-of select="$varPosition_COL9"/>
								</xsl:when>
								<xsl:when test="$varPosition_COL9 &lt; 0">
									<xsl:value-of select="$varPosition_COL9 * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						
						<SideTagValue>
							<xsl:choose>
								<xsl:when  test="COL4='B'">
									<xsl:value-of select="'1'"/>
								</xsl:when>

								<xsl:when  test="COL4='OS'">
									<xsl:value-of select="'C'"/>
								</xsl:when>

								<xsl:when  test="COL4='SS'">
									<xsl:value-of select="'5'"/>
								</xsl:when>

								<xsl:when  test="COL4='CB'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:when  test="COL4='CS'">
									<xsl:value-of select="'D'"/>
								</xsl:when>
								<xsl:when  test="COL4='OB'">
									<xsl:value-of select="'A'"/>
								</xsl:when>
								<xsl:when  test="COL4='S'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when  test="COL4='SB'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when  test="COL4='SSP'">
									<xsl:value-of select="'2'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<xsl:variable name="PB_CountnerParty" select="normalize-space(COL29)"/>
						<xsl:variable name="PRANA_CounterPartyID">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_CountnerParty]/@PranaBrokerCode"/>
						</xsl:variable>
						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="contains(COL27,'ARCA')">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="contains(COL27,'CHBC')">
									<xsl:value-of select="'7'"/>
								</xsl:when>
								<xsl:when test="contains(COL27,'DEAN')">
									<xsl:value-of select="'9'"/>
								</xsl:when>

								<xsl:when test="contains(COL27,'DFIN')">
									<xsl:value-of select="'11'"/>
								</xsl:when>

								<xsl:when test="contains(COL27,'ETRS')">
									<xsl:value-of select="'12'"/>
								</xsl:when>

								<xsl:when test="contains(COL27,'FOGS')">
									<xsl:value-of select="'14'"/>
								</xsl:when>

								<xsl:when test="contains(COL27,'NON')">
									<xsl:value-of select="'15'"/>
								</xsl:when>

								<xsl:when test="contains(COL27,'OCC')">
									<xsl:value-of select="'16'"/>
								</xsl:when>

								<xsl:when test="contains(COL27,'PUMX')">
									<xsl:value-of select="'18'"/>
								</xsl:when>

								<xsl:when test="contains(COL27,'QNTX')">
									<xsl:value-of select="'19'"/>
								</xsl:when>
								<xsl:when test="contains(COL27,'WEXM')">
									<xsl:value-of select="'20'"/>
								</xsl:when>

								<xsl:when test="contains(COL27,'RDBN')">
									<xsl:value-of select="'13'"/>
								</xsl:when>

								<xsl:when test="contains(COL27,'ISIG')">
									<xsl:value-of select="'17'"/>
								</xsl:when>

								<xsl:when test="contains(COL27,'WELX')">
									<xsl:value-of select="'19'"/>
								</xsl:when>


								<xsl:when test="$PRANA_CounterPartyID !=''">
									<xsl:value-of select ="$PRANA_CounterPartyID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>


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


