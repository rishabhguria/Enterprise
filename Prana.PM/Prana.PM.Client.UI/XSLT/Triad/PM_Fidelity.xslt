<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<DocumentElement>	

			<xsl:for-each select ="//PositionMaster">

				<xsl:if test ="number(COL13)">

					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'GS'"/>
						</xsl:variable>						

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL8"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>

								<xsl:when test ="$PRANA_SYMBOL_NAME != ''">
									<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test ="string-length(COL8)=21">
											<xsl:value-of select ="''"/>
										</xsl:when>
										
										<xsl:when test ="COL8 !='*'">
											<xsl:value-of select ="normalize-space(COL8)"/>
										</xsl:when>
										
										<xsl:otherwise>
											<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
								
							</xsl:choose>
						</Symbol>

						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test ="string-length(COL8)=21">
									<xsl:value-of select="concat(COL8,'U')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</IDCOOptionSymbol>
						
						

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<xsl:variable name="PB_BROKER_NAME">
							<xsl:value-of select="substring-before(COL18,' ')"/>
						</xsl:variable>
						
						<xsl:variable name="PRANA_BROKER_ID">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
						</xsl:variable>
						
						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="number($PRANA_BROKER_ID)">
									<xsl:value-of select="$PRANA_BROKER_ID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>

						<PositionStartDate>
							<xsl:value-of select ="COL9"/>
						</PositionStartDate>
						
						<SettlementDate>
							<xsl:value-of select ="COL10"/>
						</SettlementDate>

						<xsl:variable name ="NetPosition">
							<xsl:choose>
								
								<xsl:when test ="number(COL13) &lt; 0">
									<xsl:value-of select ="number(COL13)*-1"/>
								</xsl:when>

								<xsl:when test ="number(COL13) &gt; 0">
									<xsl:value-of select ="number(COL13)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>							
						</xsl:variable>

						<NetPosition>
							<xsl:value-of select ="$NetPosition"/>
						</NetPosition>

						<SideTagValue>

							<xsl:choose>

								<xsl:when test ="COL11='BUY'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>
								<xsl:when test ="COL11='SELL'">
									<xsl:value-of select ="'2'"/>
								</xsl:when>
								<xsl:when test ="COL11='BUY TO CLOSE'">
									<xsl:value-of select ="'B'"/>								
								</xsl:when>
								<xsl:when test ="COL11='BUY TO COVER'">
									<xsl:value-of select ="'B'"/>
								</xsl:when>
								<xsl:when test ="COL11='BUY TO OPEN'">
									<xsl:value-of select ="'A'"/>
								</xsl:when>
								<xsl:when test ="COL11='SELL TO OPEN'">
									<xsl:value-of select ="'C'"/>
								</xsl:when>
								<xsl:when test ="COL11='SHORT SELL'">
									<xsl:value-of select ="'5'"/>
								</xsl:when>
                                <xsl:when test ="COL11='SELL TO CLOSE'">
									<xsl:value-of select ="'D'"/>								
								</xsl:when>
								
							</xsl:choose>

						</SideTagValue>

						<xsl:variable name ="varCostBasis">
							<xsl:value-of select ="number(COL14)"/>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>

								<xsl:when test ="$varCostBasis &lt;0">
									<xsl:value-of select ="$varCostBasis*-1"/>
								</xsl:when>

								<xsl:when test ="$varCostBasis &gt;0">
									<xsl:value-of select ="$varCostBasis"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CostBasis>						
							
						<xsl:variable name ="varCommission">
							<xsl:value-of select ="number(COL16)"/>
						</xsl:variable>
						
						<Commission>
						<xsl:choose>

								<xsl:when test ="$varCommission &lt;0">
									<xsl:value-of select ="$varCommission*-1"/>
								</xsl:when>

								<xsl:when test ="$varCommission &gt;0">
									<xsl:value-of select ="$varCommission"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
					</Commission>

						<SecFee>
							<xsl:choose>
								<xsl:when test ="string-length(COL8)=21">
									<xsl:choose>
										<xsl:when test ="COL11='SELL TO OPEN' or COL11='SELL TO CLOSE'">
											<xsl:value-of select ="format-number(($NetPosition * $varCostBasis * 100 * 0.0000218),'#.##')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select ="0"/>
										</xsl:otherwise>
									</xsl:choose>

								</xsl:when>

								<xsl:otherwise>

									<xsl:choose>
										<xsl:when test ="COL11='SELL' or COL11='SHORT SELL'">
											<xsl:value-of select ="format-number($NetPosition * $varCostBasis * 0.0000218,'#.##')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select ="0"/>
										</xsl:otherwise>
									</xsl:choose>

								</xsl:otherwise>

							</xsl:choose>
						</SecFee>

						<OrfFee>
							<xsl:choose>
								<xsl:when test ="string-length(COL8)=21">

									<xsl:choose>
										<xsl:when test ="COL11='BUY TO OPEN' or COL11='BUY TO COVER' or COL11='SELL TO OPEN' or COL11='SELL TO CLOSE'">
											<xsl:value-of select ="format-number($NetPosition * 0.0407,'#.##')"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select ="0"/>
										</xsl:otherwise>
									</xsl:choose>
									
								</xsl:when>
								
								<xsl:otherwise>									
									<xsl:value-of select ="0"/>									
								</xsl:otherwise>
								
							</xsl:choose>
						</OrfFee>


						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

						<PositionSettlementDate>
							<xsl:value-of select="normalize-space(COL10)"/>
						</PositionSettlementDate>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>
