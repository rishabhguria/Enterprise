<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select="//Comparision">

				<xsl:if test="number(COL19) and COL7!='Cash Dividend' and COL7!='Mark to Market'">

				
					<PositionMaster>

						<xsl:variable name="varPrice">
							<xsl:value-of select="COL21"/>
						</xsl:variable>

						<xsl:variable name="varPosition">
							<xsl:value-of select="COL19"/>
						</xsl:variable>

						<xsl:variable name="varComm">
							<xsl:value-of select="COL27"/>
						</xsl:variable>
						
						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL10)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name='Fidelity']/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL3"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='Fidelity']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME=''">
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<xsl:variable name="Symbol">
							<xsl:choose>
								<xsl:when test="COL10 = '' and contains(COL15,'DEBT')">
									<xsl:value-of select="COL11"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL10"/>
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

						<Commission>
							<xsl:choose>
								<xsl:when test="number($varComm) ">
									<xsl:value-of select="$varComm"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="Gross">
							<xsl:value-of select="number(COL19) * number(COL21)"/>
						</xsl:variable>

						<xsl:variable name="TotalComm">
							<xsl:value-of select="$Gross + number(COL32)"/>
						</xsl:variable>

						<TotalCommissionandFees>

							<xsl:choose>
								<xsl:when test="COL10 = '' and contains(COL15,'DEBT')">
									<xsl:value-of select="COL27"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$TotalComm &gt; 0">
											<xsl:value-of select="$TotalComm"/>
										</xsl:when>
										<xsl:when test="$TotalComm &lt; 0" >
											<xsl:value-of select="$TotalComm*(-1)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
							
						</TotalCommissionandFees>

						<Quantity>
							<!--<xsl:choose>
								<xsl:when test="number($varPosition) ">
									<xsl:value-of select="$varPosition"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>-->
							<xsl:choose>
								<xsl:when test="number($varPosition) &gt; 0">
									<xsl:value-of select="$varPosition"/>
								</xsl:when>
								<xsl:when test="number($varPosition) &lt; 0">
									<xsl:value-of select="$varPosition*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<xsl:variable name="Side" select="normalize-space(COL7)"/>

						<Side>
							<xsl:choose>

								<xsl:when test="$Side='Buy'">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>

								<xsl:when test="$Side='Sell'">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>

								<xsl:when test="$Side='SellShort'">
									<xsl:value-of select="'Sell short'"/>
								</xsl:when>

								<xsl:when test="$Side='CoverShort'">
									<xsl:value-of select="'Buy to Close'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<AvgPX>
							<xsl:choose>
								<xsl:when test="number($varPrice) &gt; 0">
									<xsl:value-of select="$varPrice"/>
								</xsl:when>
								<xsl:when test="number($varPrice) &lt; 0">
									<xsl:value-of select="$varPrice*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>

						<NetNotionalValue>
							<xsl:choose>
								<xsl:when test="number(COL32) &gt; 0">
									<xsl:value-of select="COL32"/>
								</xsl:when>
								<xsl:when test="number(COL32) &lt; 0">
									<xsl:value-of select="COL32*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValue>

						<CurrencySymbol>
							<xsl:value-of select="COL17"/>
						</CurrencySymbol>

						<TradeDate>
							<xsl:value-of select="COL5"/>
						</TradeDate>

						<!--<Commission>
							<xsl:choose>
								<xsl:when test="number(COL10) &gt; 0">
									<xsl:value-of select="COL10"/>
								</xsl:when>
								<xsl:when test="number(COL10) &lt; 0">
									<xsl:value-of select="COL10*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<Fees>
							<xsl:value-of select="COL11+COL12"/>
						</Fees>-->

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
   
</xsl:template>

</xsl:stylesheet> 
