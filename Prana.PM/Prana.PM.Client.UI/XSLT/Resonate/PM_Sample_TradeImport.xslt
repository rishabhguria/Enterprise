<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
	<DocumentElement>
		
		<xsl:for-each select="//PositionMaster">
			
			<xsl:if test="number(COL5)">
				
				<PositionMaster>

					<xsl:variable name = "var_PB_NAME" >
						<xsl:value-of select="'ML'"/>
					</xsl:variable>
					
					<xsl:variable name = "PB_FUND_NAME" >
						<xsl:value-of select="COL21"/>
					</xsl:variable>

					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$var_PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
					</xsl:variable>

					<AccountName>
						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<xsl:value-of select="$PB_FUND_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select='$PRANA_FUND_NAME'/>
							</xsl:otherwise>
						</xsl:choose>
					</AccountName>

					<Description>
						<xsl:value-of select="COL22"/>
					</Description>

					<!--<Strategy>
						<xsl:value-of select="COL20"/>				
					</Strategy>-->
					<xsl:variable name="PB_Symbol">
						<xsl:value-of select = "translate(COL2,$varLower,$varUpper)"/> 
					</xsl:variable>
						
					<xsl:variable name="PRANA_SYMBOL_NAME">
						<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/MSPB[@Name=$var_PB_NAME]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
					</xsl:variable>

					<PBSymbol>
						<xsl:value-of select="COL2"/>
					</PBSymbol>
					
					<Symbol>
						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME=''">
								<xsl:value-of select="$PB_Symbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</Symbol>

					<PositionStartDate>
						<xsl:value-of select="COL3"/>
					</PositionStartDate>

					<SideTagValue>
						<xsl:choose>
							<xsl:when test="COL1 = 'Buy'">
								<xsl:value-of select="'1'"/>
							</xsl:when>
							<xsl:when test="COL1 = 'Sell'">
								<xsl:value-of select="'2'"/>
							</xsl:when>
														<xsl:when test="COL1 = 'Sell Short'">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:when test="COL1 = 'Buy to Close'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
																<xsl:when test="COL1 = 'Buy to Open'">
										<xsl:value-of select="'9'"/>
									</xsl:when>
									<xsl:when test="COL1 = 'Buy to Close'">
										<xsl:value-of select="'B'"/>
									</xsl:when>
							<xsl:when test="COL1 = 'Sell to Open'">
								<xsl:value-of select="'11'"/>
							</xsl:when>
							<xsl:when test="COL1 = 'Sell to Closee'">
								<xsl:value-of select="'12'"/>
							</xsl:when>
									<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SideTagValue>					

					<NetPosition>
						<xsl:choose>
							<xsl:when  test="number(normalize-space(COL5)) &gt; 0">
								<xsl:value-of select="COL5"/>
							</xsl:when>
							<xsl:when test="number(normalize-space(COL5)) &lt; 0">
								<xsl:value-of select="COL5* (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</NetPosition>

					<CostBasis>
						<xsl:choose>
							<xsl:when  test="number(normalize-space(COL17)) &gt; 0">
								<xsl:value-of select="COL17"/>
							</xsl:when>
							<xsl:when test="number(normalize-space(COL17)) &lt; 0">
								<xsl:value-of select="COL17* (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</CostBasis>

					<PositionSettlementDate>
						<xsl:value-of select="COL4"/>
					</PositionSettlementDate>

					<!--<Description>
						<xsl:value-of select="COL18"/>
					</Description>-->


					<Commission>
						<xsl:choose>
							<xsl:when  test="number(normalize-space(COL14)) &gt; 0">
								<xsl:value-of select="COL14"/>
							</xsl:when>
							<xsl:when test="number(normalize-space(COL14)) &lt; 0">
								<xsl:value-of select="COL14* (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Commission>

					<LotId>
						<xsl:value-of select="COL10"/>
					</LotId>

					<ExternalTransId>
						<xsl:value-of select="COL11"/>
					</ExternalTransId>

					<xsl:variable name="varFees">
						<xsl:choose>
							<xsl:when  test="number(normalize-space(COL15)) ">
								<xsl:value-of select="COL15"/>
							</xsl:when>							
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="varOtherFee">
						<xsl:choose>
							<xsl:when  test="number(normalize-space(COL16)) ">
								<xsl:value-of select="COL16"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<Fees>
						<xsl:value-of select="$varFees+$varOtherFee"/>
					</Fees>

					<OriginalPurchaseDate>
						<xsl:value-of select="COL19"/>
					</OriginalPurchaseDate>

					<!--<TransactionType>
						<xsl:value-of select="COL23"/>
					</TransactionType>-->
					
				</PositionMaster>
				
			</xsl:if>
			
		</xsl:for-each>
		
	</DocumentElement>
	
 </xsl:template>

	<xsl:variable name="varLower" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="varUpper" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet> 
