<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>

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

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="varQuantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL12"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test ="number($varQuantity)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL11)"/>
						</xsl:variable>						
						
						<xsl:variable name="varAsset">
							<xsl:value-of select="normalize-space(COL10)"/>
						</xsl:variable>
						
						<xsl:variable name="AssetType">
							<xsl:choose>
								<xsl:when test="$varAsset='OPTN'">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>	
                                <xsl:when test="$varAsset='EQTY'">
									<xsl:value-of select="'Equity'"/>
								</xsl:when>									
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varSEDOL">
							<xsl:choose>
								<xsl:when test="$varAsset='EQTY'">
									<xsl:value-of select="normalize-space(COL8)"/>
								</xsl:when>								
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						<xsl:variable name="varBloomberg">
							<xsl:choose>
								<xsl:when test="$varAsset='OPTN'">
									<xsl:value-of select="normalize-space(COL6)"/>
								</xsl:when>								
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>						

						<SEDOL>
							<xsl:choose>								
								<xsl:when test="$varSEDOL!=''">
									<xsl:value-of select="$varSEDOL"/>
								</xsl:when>								
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SEDOL>
						
						<Bloomberg>
							<xsl:choose>
								<xsl:when test="$varBloomberg!=''">
									<xsl:value-of select="$varBloomberg"/>
								</xsl:when>								
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Bloomberg>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL1)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<xsl:variable name="varSide" select="normalize-space(COL4)"/>						
						
						<SideTagValue>
                            <xsl:choose>
                                <xsl:when test="$AssetType='EquityOption'">
                                    <xsl:choose>
                                        <xsl:when test="$varSide='BY'">
                                            <xsl:value-of select="'A'"/>
                                        </xsl:when>
                                        <xsl:when test="$varSide='SL'">
                                            <xsl:value-of select="'D'"/>
                                        </xsl:when>
                                        <xsl:when test="$varSide='CS'">
                                            <xsl:value-of select="'B'"/>
                                        </xsl:when>
                                        <xsl:when test="$varSide='SS'">
                                            <xsl:value-of select="'C'"/>
                                        </xsl:when>
                                    </xsl:choose>
                                </xsl:when>
                                <xsl:otherwise>
                                    <xsl:choose>
                                        <xsl:when test="$varSide='BY'">
                                            <xsl:value-of select="'1'"/>
                                        </xsl:when>
                                        <xsl:when test="$varSide='SL'">
                                            <xsl:value-of select="'2'"/>
                                        </xsl:when>
                                        <xsl:when test="$varSide='CS'">
                                            <xsl:value-of select="'B'"/>
                                        </xsl:when>
                                        <xsl:when test="$varSide='SS'">
                                            <xsl:value-of select="'5'"/>
                                        </xsl:when>
                                    </xsl:choose>
                                </xsl:otherwise>
                            </xsl:choose>
                        </SideTagValue>

						<NetPosition>
							<xsl:choose>
								<xsl:when test="$varQuantity &gt; 0">
									<xsl:value-of select="$varQuantity"/>
								</xsl:when>
								<xsl:when test="$varQuantity &lt; 0">
									<xsl:value-of select="$varQuantity * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<xsl:variable name="varAvgPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL13)"/>
							</xsl:call-template>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>
								<xsl:when test="$varAvgPrice &gt; 0">
									<xsl:value-of select="$varAvgPrice"/>
								</xsl:when>
								<xsl:when test="$varAvgPrice &lt; 0">
									<xsl:value-of select="$varAvgPrice * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<xsl:variable name="varCommission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL18)"/>
							</xsl:call-template>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test="$varCommission &gt; 0">
									<xsl:value-of select="$varCommission"/>
								</xsl:when>
								<xsl:when test="$varCommission &lt; 0">
									<xsl:value-of select="$varCommission * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="varSecFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL21)"/>
							</xsl:call-template>
						</xsl:variable>

						<SecFee>
							<xsl:choose>
								<xsl:when test="$varSecFee &gt; 0">
									<xsl:value-of select="$varSecFee"/>
								</xsl:when>
								<xsl:when test="$varSecFee &lt; 0">
									<xsl:value-of select="$varSecFee * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SecFee>

						<xsl:variable name="varOtherBrokerFees">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL20)"/>
							</xsl:call-template>
						</xsl:variable>

						<OtherBrokerFees>
							<xsl:choose>
								<xsl:when test="$varOtherBrokerFees &gt; 0">
									<xsl:value-of select="$varOtherBrokerFees"/>
								</xsl:when>
								<xsl:when test="$varOtherBrokerFees &lt; 0">
									<xsl:value-of select="$varOtherBrokerFees * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</OtherBrokerFees>

						<xsl:variable name="varPositionStartDate">
							<xsl:value-of select ="normalize-space(COL2)"/>
						</xsl:variable>

						<PositionStartDate>
							<xsl:value-of select ="$varPositionStartDate"/>
						</PositionStartDate>

						<xsl:variable name="varSettlementDate">
							<xsl:value-of select ="normalize-space(COL3)"/>
						</xsl:variable>

						<PositionSettlementDate>
							<xsl:value-of select ="$varSettlementDate"/>
						</PositionSettlementDate>

						<xsl:variable name="varSettlCurrency">
							<xsl:value-of select ="normalize-space(COL22)"/>
						</xsl:variable>

						<SettlCurrencyName>
							<xsl:value-of select ="$varSettlCurrency"/>
						</SettlCurrencyName>


					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>