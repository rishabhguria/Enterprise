<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                xmlns:xs="http://www.w3.org/2001/XMLSchema"
                exclude-result-prefixes="xs">
	<xsl:output method="xml"
	            encoding="UTF-8"
	            indent="yes"/>
	<xsl:template match="/RebalancerModels">
		<RebalancerModels>
			<RebalancerModel>
				<Symbol>
					<xsl:value-of select="'Symbol'"/>
				</Symbol>
				 <BloombergSymbol>
                    <xsl:value-of select="'Identifier'"/>
                </BloombergSymbol>
				<FactSetSymbol>
					<xsl:value-of select="'FactSetSymbol'"/>
				</FactSetSymbol>
				<ActivSymbol>
					<xsl:value-of select="'ActivSymbol'"/>
				</ActivSymbol>
				<Side>
					<xsl:value-of select="'Side'"/>
				</Side>
				<AccountID>
                    <xsl:value-of select="'Fund'"/>
                </AccountID>
				<Price>
					<xsl:value-of select="'Price'"/>
				</Price>
				<FXRate>
					<xsl:value-of select="'FXRate'"/>
				</FXRate>
				<Quantity>
					<xsl:value-of select="'Quantity'"/>
				</Quantity>
				<RoundLot>
					<xsl:value-of select="'RoundLot'"/>
				</RoundLot>
				<Multiplier>
					<xsl:value-of select="'Multiplier'"/>
				</Multiplier>
				<Sector>
					<xsl:value-of select="'Sector'"/>
				</Sector>
				<Asset>
					<xsl:value-of select="'Asset'"/>
				</Asset>
				<Delta>
					<xsl:value-of select="'Delta'"/>
				</Delta>
				<LeveragedFactor>
					<xsl:value-of select="'LeveragedFactor'"/>
				</LeveragedFactor>
				<IsStaleClosingMark>
					<xsl:value-of select="'IsStaleClosingMark'"/>
				</IsStaleClosingMark>
				<IsStaleFxRate>
					<xsl:value-of select="'IsStaleFxRate'"/>
				</IsStaleFxRate>
				<AvgPrice>
					<xsl:value-of select="'AvgPrice'"/>
				</AvgPrice>
				<CostBasisPNL>
					<xsl:value-of select="'CostBasisPNL'"/>
				</CostBasisPNL>
				<AUECID>
					<xsl:value-of select="'AUECID'"/>
				</AUECID>
				<IsCalculatedModel>
					<xsl:value-of select="'IsCalculatedModel'"/>
				</IsCalculatedModel>
				<IsModified>
					<xsl:value-of select="'IsModified'"/>
				</IsModified>
				<IsCustomModel>
					<xsl:value-of select="'IsCustomModel'"/>
				</IsCustomModel>
				<IsNewlyAdded>
					<xsl:value-of select="'IsNewlyAdded'"/>
				</IsNewlyAdded>
				<PriceInBaseCurrency>
					<xsl:value-of select="'PriceInBaseCurrency'"/>
				</PriceInBaseCurrency>
				<CurrentMarketValueBase>
					<xsl:value-of select="'CurrentMarketValueBase'"/>
				</CurrentMarketValueBase>
				<CurrentMarketValueLocal>
					<xsl:value-of select="'CurrentMarketValueLocal'"/>
				</CurrentMarketValueLocal>
				<RebalCalculationLevel>
					<xsl:value-of select="'RebalCalculationLevel'"/>
				</RebalCalculationLevel>
				<CurrentPercentage>
					<xsl:value-of select="'CurrentPercentage'"/>
				</CurrentPercentage>
				<ChangePercentage>
					<xsl:value-of select="'ChangePercentage'"/>
				</ChangePercentage>
				<CurrentAccountLevelPercentage>
					<xsl:value-of select="'CurrentAccountLevelPercentage'"/>
				</CurrentAccountLevelPercentage>
				<CurrentAccountGroupLevelPercentage>
					<xsl:value-of select="'CurrentAccountGroupLevelPercentage'"/>
				</CurrentAccountGroupLevelPercentage>
				 <TargetPercentage>
                    <xsl:value-of select="'Target Weigh'"/>
                </TargetPercentage>
				<TargetAccountLevelPercentage>
					<xsl:value-of select="'TargetAccountLevelPercentage'"/>
				</TargetAccountLevelPercentage>
				<TargetAccountGroupLevelPercentage>
					<xsl:value-of select="'TargetAccountGroupLevelPercentage'"/>
				</TargetAccountGroupLevelPercentage>
				<TargetPosition>
					<xsl:value-of select="'TargetPosition'"/>
				</TargetPosition>
				<TargetMarketValueBase>
					<xsl:value-of select="'TargetMarketValueBase'"/>
				</TargetMarketValueBase>
				<TargetMarketValueLocal>
					<xsl:value-of select="'TargetMarketValueLocal'"/>
				</TargetMarketValueLocal>
				<BuySellValue>
					<xsl:value-of select="'BuySellValue'"/>
				</BuySellValue>
				<BuySellQty>
					<xsl:value-of select="'BuySellQty'"/>
				</BuySellQty>
				<IsLock>
					<xsl:value-of select="'IsLock'"/>
				</IsLock>
				<Commission>
					<xsl:value-of select="'Commission'"/>
				</Commission>
				<CurrentDate>
                    <xsl:value-of select="'Trade Date'"/>
                </CurrentDate>
			</RebalancerModel>
			<xsl:for-each select="RebalancerModel[TargetPercentage!=0]">
				<RebalancerModel>
					<Symbol>
						<xsl:value-of select="Symbol"/>
					</Symbol>
					<BloombergSymbol>
						<xsl:choose>
							<xsl:when test="BloombergSymbol = '*'">
								<xsl:value-of select="'USD'"/>
							</xsl:when>
							<xsl:when test="BloombergSymbol != ''">
								<xsl:value-of select="BloombergSymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>
					</BloombergSymbol>
					<FactSetSymbol>
						<xsl:value-of select="FactSetSymbol"/>
					</FactSetSymbol>
					<ActivSymbol>
						<xsl:value-of select="ActivSymbol"/>
					</ActivSymbol>
					<Side>
						<xsl:value-of select="Side"/>
					</Side>
					<AccountId>
						<xsl:value-of select="AccountId"/>
					</AccountId>
					<Price>
						<xsl:value-of select="Price"/>
					</Price>
					<FXRate>
						<xsl:value-of select="FXRate"/>
					</FXRate>
					<Quantity>
						<xsl:value-of select="Quantity"/>
					</Quantity>
					<RoundLot>
						<xsl:value-of select="RoundLot"/>
					</RoundLot>
					<Multiplier>
						<xsl:value-of select="Multiplier"/>
					</Multiplier>
					<Sector>
						<xsl:value-of select="Sector"/>
					</Sector>
					<Asset>
						<xsl:value-of select="Asset"/>
					</Asset>
					<Delta>
						<xsl:value-of select="Delta"/>
					</Delta>
					<LeveragedFactor>
						<xsl:value-of select="LeveragedFactor"/>
					</LeveragedFactor>
					<IsStaleClosingMark>
						<xsl:value-of select="IsStaleClosingMark"/>
					</IsStaleClosingMark>
					<IsStaleFxRate>
						<xsl:value-of select="IsStaleFxRate"/>
					</IsStaleFxRate>
					<AvgPrice>
						<xsl:value-of select="AvgPrice"/>
					</AvgPrice>
					<CostBasisPNL>
						<xsl:value-of select="CostBasisPNL"/>
					</CostBasisPNL>
					<AUECID>
						<xsl:value-of select="AUECID"/>
					</AUECID>
					<IsCalculatedModel>
						<xsl:value-of select="IsCalculatedModel"/>
					</IsCalculatedModel>
					<IsModified>
						<xsl:value-of select="IsModified"/>
					</IsModified>
					<IsCustomModel>
						<xsl:value-of select="IsCustomModel"/>
					</IsCustomModel>
					<IsNewlyAdded>
						<xsl:value-of select="IsNewlyAdded"/>
					</IsNewlyAdded>
					<PriceInBaseCurrency>
						<xsl:value-of select="PriceInBaseCurrency"/>
					</PriceInBaseCurrency>
					<CurrentMarketValueBase>
						<xsl:value-of select="CurrentMarketValueBase"/>
					</CurrentMarketValueBase>
					<CurrentMarketValueLocal>
						<xsl:value-of select="CurrentMarketValueLocal"/>
					</CurrentMarketValueLocal>
					<RebalCalculationLevel>
						<xsl:value-of select="RebalCalculationLevel"/>
					</RebalCalculationLevel>
					<CurrentPercentage>
						<xsl:value-of select="CurrentPercentage"/>
					</CurrentPercentage>
					<ChangePercentage>
						<xsl:value-of select="ChangePercentage"/>
					</ChangePercentage>
					<CurrentAccountLevelPercentage>
						<xsl:value-of select="CurrentAccountLevelPercentage"/>
					</CurrentAccountLevelPercentage>
					<CurrentAccountGroupLevelPercentage>
						<xsl:value-of select="CurrentAccountGroupLevelPercentage"/>
					</CurrentAccountGroupLevelPercentage>
					<TargetPercentage>
						<xsl:value-of select="format-number(TargetPercentage, '0.0000')"/>
					</TargetPercentage>
					<TargetAccountLevelPercentage>
						<xsl:value-of select="TargetAccountLevelPercentage"/>
					</TargetAccountLevelPercentage>
					<TargetAccountGroupLevelPercentage>
						<xsl:value-of select="TargetAccountGroupLevelPercentage"/>
					</TargetAccountGroupLevelPercentage>
					<TargetPosition>
						<xsl:value-of select="TargetPosition"/>
					</TargetPosition>
					<TargetMarketValueBase>
						<xsl:value-of select="TargetMarketValueBase"/>
					</TargetMarketValueBase>
					<TargetMarketValueLocal>
						<xsl:value-of select="TargetMarketValueLocal"/>
					</TargetMarketValueLocal>
					<BuySellValue>
						<xsl:value-of select="BuySellValue"/>
					</BuySellValue>
					<BuySellQty>
						<xsl:value-of select="BuySellQty"/>
					</BuySellQty>
					<IsLock>
						<xsl:value-of select="IsLock"/>
					</IsLock>
					<Commission>
						<xsl:value-of select="Commission"/>
					</Commission>
					<CurrentDate>
						<xsl:value-of select="CurrentDate"/>
					</CurrentDate>
				</RebalancerModel>
			</xsl:for-each>
		</RebalancerModels>
	</xsl:template>
</xsl:stylesheet>
