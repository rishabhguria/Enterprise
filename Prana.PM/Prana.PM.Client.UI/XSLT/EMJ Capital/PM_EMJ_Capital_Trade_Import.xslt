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

		<!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
	</xsl:template>




	<xsl:template name="MonthCode">
		<xsl:param name="Month" />
		<xsl:param name="PutOrCall" />
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'A'" />
				</xsl:when>
				<xsl:when test="$Month='02'">
					<xsl:value-of select="'B'" />
				</xsl:when>
				<xsl:when test="$Month='03'">
					<xsl:value-of select="'C'" />
				</xsl:when>
				<xsl:when test="$Month='04'">
					<xsl:value-of select="'D'" />
				</xsl:when>
				<xsl:when test="$Month='05'">
					<xsl:value-of select="'E'" />
				</xsl:when>
				<xsl:when test="$Month='06'">
					<xsl:value-of select="'F'" />
				</xsl:when>
				<xsl:when test="$Month='07'">
					<xsl:value-of select="'G'" />
				</xsl:when>
				<xsl:when test="$Month='08'">
					<xsl:value-of select="'H'" />
				</xsl:when>
				<xsl:when test="$Month='09'">
					<xsl:value-of select="'I'" />
				</xsl:when>
				<xsl:when test="$Month='10'">
					<xsl:value-of select="'J'" />
				</xsl:when>
				<xsl:when test="$Month='11'">
					<xsl:value-of select="'K'" />
				</xsl:when>
				<xsl:when test="$Month='12'">
					<xsl:value-of select="'L'" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'M'" />
				</xsl:when>
				<xsl:when test="$Month='02'">
					<xsl:value-of select="'N'" />
				</xsl:when>
				<xsl:when test="$Month='03'">
					<xsl:value-of select="'O'" />
				</xsl:when>
				<xsl:when test="$Month='04'">
					<xsl:value-of select="'P'" />
				</xsl:when>
				<xsl:when test="$Month='05'">
					<xsl:value-of select="'Q'" />
				</xsl:when>
				<xsl:when test="$Month='06'">
					<xsl:value-of select="'R'" />
				</xsl:when>
				<xsl:when test="$Month='07'">
					<xsl:value-of select="'S'" />
				</xsl:when>
				<xsl:when test="$Month='08'">
					<xsl:value-of select="'T'" />
				</xsl:when>
				<xsl:when test="$Month='09'">
					<xsl:value-of select="'U'" />
				</xsl:when>
				<xsl:when test="$Month='10'">
					<xsl:value-of select="'V'" />
				</xsl:when>
				<xsl:when test="$Month='11'">
					<xsl:value-of select="'W'" />
				</xsl:when>
				<xsl:when test="$Month='12'">
					<xsl:value-of select="'X'" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>


	<xsl:template name="Option">
		<xsl:param name="varSymbol" />
		
		
		
		<xsl:variable name="SubSymbol">
			<xsl:value-of select="substring-after(COL4,substring(COL4,1,6))" />
		</xsl:variable>
		
		

		<xsl:variable name="PutORCall">
			
		<xsl:choose>
				<xsl:when test="contains($SubSymbol,'C')">
					<xsl:value-of select="'C'" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'P'" />
				</xsl:otherwise>
			</xsl:choose>
		
		</xsl:variable>
		
		<xsl:variable name="ValueBeforePutORCall">
		<xsl:choose>
				<xsl:when test="contains($SubSymbol,'C')">
					<xsl:value-of select="concat(substring(COL4,1,5),substring-before(substring(COL4,6),'C'))" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="concat(substring(COL4,1,5),substring-before(substring(COL4,6),'P'))" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		

		
		
		<xsl:variable name="UnderlyingSymbol">
			<xsl:choose>
		       <xsl:when test="contains($SubSymbol,'C')">
				<xsl:value-of select="substring(concat(substring(COL4,1,5),substring-before(substring(COL4,6),'C')),1,string-length(concat(substring(COL4,1,5),substring-before(substring(COL4,6),'C')))-6)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="substring(concat(substring(COL4,1,5),substring-before(substring(COL4,6),'P')),1,string-length(concat(substring(COL4,1,5),substring-before(substring(COL4,6),'P')))-6)"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		
		<xsl:variable name="ExpiryDay">
			<xsl:value-of select="substring($ValueBeforePutORCall,string-length($ValueBeforePutORCall)-1,2)" />
		</xsl:variable>
		
		<xsl:variable name="ExpiryMonth">
			<xsl:value-of select="substring($ValueBeforePutORCall,string-length($ValueBeforePutORCall)-3,2)" />
		</xsl:variable>


		<xsl:variable name="ExpiryYear">

			<xsl:value-of select="substring($ValueBeforePutORCall,string-length($ValueBeforePutORCall)-5,2)" />
		</xsl:variable>
		
		<xsl:variable name="StrikePrice">
			<xsl:value-of select="format-number(substring(substring-after(COL4,$ValueBeforePutORCall),2,8),'#.00')"/>
		</xsl:variable>

		<xsl:variable name="MonthCodVar">
			<xsl:call-template name="MonthCode">
				<xsl:with-param name="Month" select="$ExpiryMonth" />
				<xsl:with-param name="PutOrCall" select="$PutORCall" />
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="Day">
			<xsl:choose>
				<xsl:when test="substring($ExpiryDay,1,1)='0'">
					<xsl:value-of select="substring($ExpiryDay,7,2)" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$ExpiryDay" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<!--<xsl:value-of select="concat('O:',$UnderlyingSymbol,$PutORCall)" />-->
		 <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ', $ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)" /> 
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="varQuantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL5"/>
					</xsl:call-template>
				</xsl:variable>				
				<xsl:if test ="number($varQuantity)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varSymbol">
							<xsl:value-of select="COL4"/>
						</xsl:variable>

						<xsl:variable name="varSEDOL">
							<xsl:value-of select="(normalize-space(COL4))" />
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="AssetType">
							<xsl:choose>
								<xsl:when test="contains(substring-after(COL4,substring(COL4,1,6)),'C') or contains(substring-after(COL4,substring(COL4,1,6)),'P')">
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
								<xsl:when test="$AssetType='EquityOption'">
									<xsl:call-template name="Option">
										<xsl:with-param name="varSymbol" select="normalize-space(COL4)" />
									</xsl:call-template>
								</xsl:when>
								<xsl:when test="string-length($varSEDOL)=7 ">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="$varSymbol!='*' or $varSymbol!=''">
									<xsl:value-of select="$varSymbol"/>
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
								<xsl:when test="string-length($varSEDOL)=7 ">
									<xsl:value-of select="$varSEDOL"/>
								</xsl:when>							
								<xsl:when test="$varSymbol!='*'">
									<xsl:value-of select="''" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SEDOL>


						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL2"/>
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

						<xsl:variable name="varSide" select="COL3"/>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$AssetType='EquityOption'">
									<xsl:choose>
										<xsl:when test="$varSide='Buy' or $varSide='BY'">
											<xsl:value-of select="'A'"/>
										</xsl:when>
										<xsl:when test="$varSide='Sell' or $varSide='SL'">
											<xsl:value-of select="'D'"/>
										</xsl:when>
										<xsl:when test="$varSide='Buy to Close' or $varSide='Cover Short'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="$varSide='Sell short' or $varSide='Sell to Open'">
											<xsl:value-of select="'C'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$varSide='Buy' or $varSide='BY'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when test="$varSide='Sell' or $varSide='SL'">
											<xsl:value-of select="'2'"/>
										</xsl:when>
										<xsl:when test="$varSide='Buy to Close' or $varSide='Cover Short'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="$varSide='Sell short' or $varSide='Sell to Open'">
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

						<xsl:variable name="varCostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL6"/>
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

						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL9"/>
							</xsl:call-template>
						</xsl:variable>
						<Commission>
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
						</Commission>

						<xsl:variable name="varsecFees">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL10"/>
							</xsl:call-template>
						</xsl:variable>


						<SecFee>
							<xsl:choose>
								<xsl:when test=" $varsecFees  &gt; 0">
									<xsl:value-of select="$varsecFees"/>
								</xsl:when>
								<xsl:when test=" $varsecFees &lt; 0">
									<xsl:value-of select="$varsecFees * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SecFee>

						<xsl:variable name="PB_BROKER_NAME">
							<xsl:value-of select="normalize-space(COL13)"/>
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

						

						<xsl:variable name="CurrencyName">
							<xsl:value-of select="normalize-space(COL12)"/>
						</xsl:variable>
						<SettlCurrencyName>
							<xsl:value-of select="$CurrencyName"/>
						</SettlCurrencyName>

						<xsl:variable name="TradeYear">
							<xsl:value-of select="substring(normalize-space(COL14),1,4)"/>
						</xsl:variable>
						<xsl:variable name="TradeMonth">
							<xsl:value-of select="substring(normalize-space(COL14),6,2)"/>
						</xsl:variable>
						<xsl:variable name="TradeDate">
							<xsl:value-of select="substring(normalize-space(COL14),9,2)"/>
						</xsl:variable>


						<xsl:variable name="varPositionStartDate">
							<xsl:value-of select ="concat($TradeMonth,'/',$TradeDate,'/',$TradeYear)"/>					
						</xsl:variable>

						<PositionStartDate>
							<xsl:value-of select="$varPositionStartDate"/>
						</PositionStartDate>

						<xsl:variable name="SettYear">
							<xsl:value-of select="substring(normalize-space(COL15),1,4)"/>
						</xsl:variable>
						<xsl:variable name="SettMonth">
							<xsl:value-of select="substring(normalize-space(COL15),6,2)"/>
						</xsl:variable>
						<xsl:variable name="SettDate">
							<xsl:value-of select="substring(normalize-space(COL15),9,2)"/>
						</xsl:variable>

						<xsl:variable name="varSettlementDate">
							<xsl:value-of select ="concat($SettMonth,'/',$SettDate,'/',$SettYear)"/>
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