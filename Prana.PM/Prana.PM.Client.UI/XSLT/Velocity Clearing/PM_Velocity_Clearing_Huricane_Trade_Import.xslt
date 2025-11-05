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

	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month='02' ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month='03' ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month='04' ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month='05' ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month='06' ">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month='07'  ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month='08'  ">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month='09' ">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month='10' ">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month='11' ">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month='12' ">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month='02' ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month='03' ">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month='04' ">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month='05' ">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month='06' ">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month='07'  ">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month='08'  ">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month='09' ">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month='10' ">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month='11' ">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month='12' ">
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
			<xsl:value-of select="substring-before(normalize-space(COL1),' ')"/>
		</xsl:variable>
		<xsl:variable name="ExpiryYear">
			<xsl:value-of select="substring-before(substring-after(substring-after(COL1,'/'),'/'),' ')"/>
		</xsl:variable>
		<xsl:variable name="ExpiryMonth">
			<xsl:value-of select="substring-before(substring-after(substring-after(COL1,' '),' '),'/')"/>
		</xsl:variable>
		<xsl:variable name="ExpiryDay">
			<xsl:value-of select="substring-before(substring-after(COL1,'/'),'/')"/>
		</xsl:variable>
		<xsl:variable name="PutORCall">
			<xsl:value-of select="substring(substring-after(substring-after(substring-after(COL1,'/'),'/'),' '),1,1)"/>
		</xsl:variable>
		<xsl:variable name="StrikePrice">
			<xsl:value-of select="format-number(substring(substring-after(substring-after(substring-after(COL1,'/'),'/'),' '),2),'##.00')"/>
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

				<xsl:variable name="varQuantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL10"/>
					</xsl:call-template>
				</xsl:variable>
				
				<xsl:if test ="number($varQuantity)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'GS'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL25)"/>
						</xsl:variable>

						<xsl:variable name="varAssetType">
							<xsl:choose>
								<xsl:when test="(COL27='PUT') or (COL27='CALL')">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						<xsl:variable name="varSymbol">
							<xsl:choose>
								<xsl:when test="$varAssetType='EquityOption'">
									<xsl:value-of select="normalize-space(COL1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varSEDOL">
							<xsl:choose>
								<xsl:when test="$varAssetType='Equity'">
									<xsl:value-of select="normalize-space(COL30)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
					
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>	

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varAssetType='EquityOption'">
									<xsl:call-template name="Option">
										<xsl:with-param name="Symbol" select="normalize-space(COL8)"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:when test="$varSEDOL!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="$varSymbol!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>					

						<SEDOL>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="$varSEDOL!=''">
									<xsl:value-of select="$varSEDOL"/>
								</xsl:when>
								<xsl:when test="$varSymbol!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SEDOL>						

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL9)"/>
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

						<xsl:variable name="varCostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL11"/>
							</xsl:call-template>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>
								<xsl:when test ="$varCostBasis &lt;0">
									<xsl:value-of select ="$varCostBasis* -1"/>
								</xsl:when>
								<xsl:when test ="$varCostBasis &gt;0">
									<xsl:value-of select ="$varCostBasis"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>
						
						<xsl:variable name="varSecFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL12"/>
							</xsl:call-template>
						</xsl:variable>

						<SecFee>
							<xsl:choose>
								<xsl:when test ="$varSecFee &lt;0">
									<xsl:value-of select ="$varSecFee* -1"/>
								</xsl:when>
								<xsl:when test ="$varSecFee &gt;0">
									<xsl:value-of select ="$varSecFee"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SecFee>
						
						<xsl:variable name="varCommission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL14"/>
							</xsl:call-template>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test ="$varCommission &lt;0">
									<xsl:value-of select ="$varCommission* -1"/>
								</xsl:when>
								<xsl:when test ="$varCommission &gt;0">
									<xsl:value-of select ="$varCommission"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="varSide">
							<xsl:value-of select="normalize-space(COL3)"/>
						</xsl:variable>

						<SideTagValue>						
								<xsl:choose>
									<xsl:when test="$varAssetType='EquityOption'">
										<xsl:choose>
											<xsl:when test="$varSide='Buy'">
												<xsl:value-of select="'A'"/>
											</xsl:when>
											<xsl:when test="$varSide='Sell'">
												<xsl:value-of select="'D'"/>
											</xsl:when>
											<xsl:when test="$varSide='Short'">
												<xsl:value-of select="'C'"/>
											</xsl:when>
											<xsl:when test="$varSide='Cover'">
												<xsl:value-of select="'B'"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="''"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="$varSide='Buy' ">
												<xsl:value-of select="'1'"/>
											</xsl:when>
											<xsl:when test="$varSide='Sell' ">
												<xsl:value-of select="'2'"/>
											</xsl:when>
											<xsl:when test="$varSide='Short' ">
												<xsl:value-of select="'5'"/>
											</xsl:when>
											<xsl:when test="$varSide='Cover' ">
												<xsl:value-of select="'B'"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="''"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>						
						</SideTagValue>
						
						<xsl:variable name="PB_BROKER_NAME">
							<xsl:value-of select="normalize-space(COL7)"/>
						</xsl:variable>
						
						<xsl:variable name="PRANA_BROKER_ID">
							<xsl:value-of select="document('../ReconMappingXml/CounterPartyMapping.xml')/CounterPartyMapping/PB[@Name=$PB_NAME]/CounterPartyData[@MappedBrokerCode=$PB_BROKER_NAME]/@BrokerCode"/>
						</xsl:variable>
						
						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="number($PRANA_BROKER_ID!='')">
									<xsl:value-of select="$PRANA_BROKER_ID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>

						<xsl:variable name="varPositionStartDate">
							<xsl:value-of select ="COL5"/>
						</xsl:variable>

						<PositionStartDate>
							<xsl:value-of select="$varPositionStartDate"/>
						</PositionStartDate>

						<xsl:variable name="varSettlementDate">
                            <xsl:value-of select ="''"/>
                        </xsl:variable>

                     <PositionSettlementDate>
                       <xsl:value-of select ="$varSettlementDate"/>
                     </PositionSettlementDate>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>