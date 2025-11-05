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

	<xsl:template name="MonthColumn">
		<xsl:param name="MonthSett"/>				
			<xsl:choose>
				<xsl:when test="$MonthSett='Jan'">
					<xsl:value-of select="'01'"/>
				</xsl:when>
				<xsl:when test="$MonthSett='Feb' ">
					<xsl:value-of select="'02'"/>
				</xsl:when>
				<xsl:when test="$MonthSett='Mar' ">
					<xsl:value-of select="'03'"/>
				</xsl:when>
				<xsl:when test="$MonthSett='Apr' ">
					<xsl:value-of select="'04'"/>
				</xsl:when>
				<xsl:when test="$MonthSett='May' ">
					<xsl:value-of select="'05'"/>
				</xsl:when>
				<xsl:when test="$MonthSett='Jun' ">
					<xsl:value-of select="'06'"/>
				</xsl:when>
				<xsl:when test="$MonthSett='Jul'  ">
					<xsl:value-of select="'07'"/>
				</xsl:when>
				<xsl:when test="$MonthSett='Aug'  ">
					<xsl:value-of select="'08'"/>
				</xsl:when>
				<xsl:when test="$MonthSett='Sep' ">
					<xsl:value-of select="'09'"/>
				</xsl:when>
				<xsl:when test="$MonthSett='Oct' ">
					<xsl:value-of select="'10'"/>
				</xsl:when>
				<xsl:when test="$MonthSett='Nov' ">
					<xsl:value-of select="'11'"/>
				</xsl:when>
				<xsl:when test="$MonthSett='Dec' ">
					<xsl:value-of select="'12'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:variable name="UnderlyingSymbol">
			<xsl:value-of select="substring-before(normalize-space(substring-after(normalize-space(COL5),'-')),' ')"/>
		</xsl:variable>
		
		<xsl:variable name="ExpiryDay">
			<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL5),'EXP'),'/'),'/')"/>
		</xsl:variable>
		
		<xsl:variable name="ExpiryMonth">
			<xsl:value-of select="substring-before(normalize-space(substring-after(normalize-space(COL5),'EXP')),'/')"/>
		</xsl:variable>
		
		<xsl:variable name="ExpiryYear">
			<xsl:value-of select="substring(substring-after(substring-after(substring-after(normalize-space(COL5),'EXP'),'/'),'/'),3,2)"/>
		</xsl:variable>
		
		<xsl:variable name="PutORCall">
			<xsl:value-of select="substring(substring-before(normalize-space(COL5),' '),1,1)"/>
		</xsl:variable>	
		
		<xsl:variable name="StrikePrice1">
			<xsl:value-of select="normalize-space(substring-before(normalize-space(substring-after(normalize-space(COL5),'@')),'EXP'))"/>
		</xsl:variable>	
		
		<xsl:variable name="varDivideSlash">
			 <xsl:value-of select="normalize-space(substring($StrikePrice1,string-length($StrikePrice1)-3))"/>
		</xsl:variable>		
		
		<xsl:variable name="varPreDecimal">
			 <xsl:value-of select="normalize-space(substring($StrikePrice1,1,string-length($StrikePrice1)-3))"/>
		</xsl:variable>	
		
		<xsl:variable name="varDecimal">
			 <xsl:value-of select="substring($varDivideSlash,1,1) div substring($varDivideSlash,3,1)"/>
		</xsl:variable>
		
		<xsl:variable name="StrikePrice">
		   <xsl:choose>
				<xsl:when test="contains($StrikePrice1,'/')">
					<xsl:value-of select="format-number($varPreDecimal + $varDecimal,'##.00')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="format-number($StrikePrice1,'##.00')"/>
				</xsl:otherwise>
			</xsl:choose>
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
				
				<xsl:variable name="varSide" select="normalize-space(COL6)"/>

				<xsl:if test ="number($varQuantity) and (COL3 = 'Cash Securities' or COL3 = 'Equity Swaps') and (contains($varSide,'Buy') or contains($varSide,'Sell'))">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'MS'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL3)"/>
						</xsl:variable>

						<xsl:variable name="varAsset">
							<xsl:value-of select="normalize-space(COL5)"/>
						</xsl:variable>

						<xsl:variable name="varAssetType">
							<xsl:choose>
								<xsl:when test="contains($varAsset,'CALL')  or contains($varAsset,'PUT')">
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

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						
						<xsl:variable name="varSedol">
							<xsl:value-of select="normalize-space(COL19)"/>
						</xsl:variable>
						
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME != ''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varAssetType='EquityOption'">
									<xsl:call-template name="Option">
										<xsl:with-param name="Symbol" select="normalize-space($varSymbol)"/>
									</xsl:call-template>
								</xsl:when>								
								<xsl:when test="$varAssetType = 'Equity' and ($varSedol = '' and $varSedol = '*') and $varSymbol != ''">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>						
						
						<SEDOL>
							<xsl:choose>								
								<xsl:when test="$varAssetType='Equity' and $varSedol != '' and $varSedol != '*'">
									<xsl:value-of select="$varSedol"/>
								</xsl:when>								
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SEDOL>
						
						<PBSymbol>
							<xsl:value-of select ="concat('Asset: ' ,$varAssetType, ', COL5: ', COL5, ',  SEDOL: ', $varSedol) "/>
						</PBSymbol>
						
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL2)"/>
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

						<SideTagValue>						
							<xsl:choose>
								<xsl:when test="$varAssetType='EquityOption'">
									<xsl:choose>
										<xsl:when test="$varSide='Buy Long'">
											<xsl:value-of select="'A'"/>
										</xsl:when>
										<xsl:when test="$varSide='Sell Long'">
											<xsl:value-of select="'D'"/>
										</xsl:when>
										<xsl:when test="$varSide='Sell Short'">
											<xsl:value-of select="'C'"/>
										</xsl:when>
										<xsl:when test="$varSide='Buy to Cover'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$varSide='Buy Long'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when test="$varSide='Sell Long'">
											<xsl:value-of select="'2'"/>
										</xsl:when>
										<xsl:when test="$varSide='Sell Short'">
											<xsl:value-of select="'5'"/>
										</xsl:when>
										<xsl:when test="$varSide='Buy to Cover'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
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
												
						<xsl:variable name="varCommission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL13)"/>
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
								<xsl:with-param name="Number" select="normalize-space(COL14)"/>
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
						
						<xsl:variable name="varPrincipalAmount">
							 <xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL11)"/>
							</xsl:call-template>
						</xsl:variable>
						
						<xsl:variable name="varCheckZeroQuantity">
							 <xsl:choose>
								<xsl:when test="$varQuantity != 0 and $varAssetType = 'Equity'">						
								 <xsl:call-template name="Translate">
								     <xsl:with-param name="Number" select="$varPrincipalAmount div $varQuantity"/>
							     </xsl:call-template>
								</xsl:when>	
								<xsl:when test="$varQuantity != 0 and $varAssetType = 'EquityOption'">						
								 <xsl:call-template name="Translate">
								     <xsl:with-param name="Number" select="$varPrincipalAmount div ($varQuantity * 100)"/>
							     </xsl:call-template>
								</xsl:when>									
								<xsl:otherwise>
									<xsl:value-of select="0"/>							
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>		
						
						<xsl:variable name="varAvgPrice">
							 <xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space($varCheckZeroQuantity)"/>
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
						
						
						<xsl:variable name="var1">							
						 <xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL12)"/>
							</xsl:call-template>
						</xsl:variable>
						
						<xsl:variable name="varOtherBrokerFee">							
						 <xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(($varPrincipalAmount + $varCommission + $varSecFee) - $var1)"/>
							</xsl:call-template>
						</xsl:variable>

						<Fees>
							<xsl:choose>
								<xsl:when test="$varOtherBrokerFee &gt; 0">									
									<xsl:value-of select="format-number($varOtherBrokerFee,'##.00000000')"/>	
								</xsl:when>
								<xsl:when test="$varOtherBrokerFee &lt; 0">								
									<xsl:value-of select="format-number($varOtherBrokerFee * (-1),'##.00000000')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>
						
						<CounterPartyID>							
						  <xsl:choose>
								<xsl:when test="normalize-space(COL16)='1900'">
									<xsl:value-of select="47"/>
								</xsl:when>
					        	<xsl:when test="normalize-space(COL16)='MSCO'">
					        			<xsl:value-of select="47"/>
					        	</xsl:when>
					        	<xsl:when test="normalize-space(COL16)='MSPB'">
					        		<xsl:value-of select="47"/>
					        	</xsl:when>
					        	<xsl:when test="normalize-space(COL16)='OCCC'">
					        		<xsl:value-of select="47"/>
					            </xsl:when>
					        	<xsl:when test="normalize-space(COL16)='JONE'">
					        	    <xsl:value-of select="62"/>
					        	</xsl:when>
					        	<xsl:when test="normalize-space(COL16)='JSTD'">
					        	   <xsl:value-of select="62"/>
					        	</xsl:when>
					            <xsl:when test="normalize-space(COL16)='MLCO'">
					        		<xsl:value-of select="62"/>
					        	</xsl:when>
					        	<xsl:when test="normalize-space(COL16)='MLCQ'">
					        		<xsl:value-of select="62"/>
					        	</xsl:when>
					            <xsl:when test="normalize-space(COL16)='EVRC'">
					        		<xsl:value-of select="16"/>
					        	</xsl:when>
					           <xsl:when test="normalize-space(COL16)='ISGR'">
					          		<xsl:value-of select="16"/>
					          	</xsl:when>
					           <xsl:when test="normalize-space(COL16)='ISIG'">
					          		<xsl:value-of select="16"/>
					          	</xsl:when>
					           <xsl:when test="normalize-space(COL16)='GSCO'">
					          		<xsl:value-of select="18"/>
					          	</xsl:when>					         
					          	<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>
						
						<xsl:variable name="varSettlCurrencyName">
							<xsl:value-of select ="normalize-space(COL15)"/>
						</xsl:variable>

						<SettlCurrencyName>
							<xsl:value-of select ="$varSettlCurrencyName"/>
						</SettlCurrencyName>

						<xsl:variable name="varOrgDate">
							<xsl:value-of select="substring-before(COL7,' ')"/>
						</xsl:variable>

						<xsl:variable name="varOrgMonth">
							<xsl:value-of select="substring(substring-after(normalize-space(COL7),' '),1,3)"/>
						</xsl:variable>

						<xsl:variable name="varOrgYear">
							<xsl:value-of select="substring-after(substring-after(COL7,' '),' ')"/>
						</xsl:variable>

						<xsl:variable name="VarMonth">
							<xsl:call-template name="MonthColumn">
								<xsl:with-param name="MonthSett" select="($varOrgMonth)"/>
						</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varFullDate">
							<xsl:value-of select="concat($VarMonth,'/',$varOrgDate,'/',$varOrgYear)"/>
						</xsl:variable>
						

						<PositionStartDate>
							<xsl:value-of select ="$varFullDate"/>
						</PositionStartDate>

						<xsl:variable name="varSettOrgDate">
                            <xsl:value-of select="substring-before(normalize-space(COL8),' ')"/>
                        </xsl:variable>

                        <xsl:variable name="varSettOrgMonth">
                            <xsl:value-of select="substring(substring-after(normalize-space(COL8),' '),1,3)"/>
                        </xsl:variable>

                        <xsl:variable name="varSettOrgYear">
                            <xsl:value-of select="substring-after(substring-after(normalize-space(COL8),' '),' ')"/>
                        </xsl:variable>

                        <xsl:variable name="VarMonthSett">
                            <xsl:call-template name="MonthColumn">
                                <xsl:with-param name="MonthSett" select="($varSettOrgMonth)"/>
                            </xsl:call-template>
                        </xsl:variable>
                        
                        <xsl:variable name="varPositionSettlementDate">
                            <xsl:value-of select="concat($VarMonthSett,'/',$varSettOrgDate,'/',$varSettOrgYear)"/>
                        </xsl:variable>                        

                        <PositionSettlementDate>
                            <xsl:value-of select ="$varPositionSettlementDate"/>
                        </PositionSettlementDate>

						

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>